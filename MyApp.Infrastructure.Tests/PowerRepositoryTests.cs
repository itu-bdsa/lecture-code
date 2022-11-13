namespace MyApp.Infrastructure.Tests;

public sealed class PowerRepositoryTests : IAsyncDisposable
{
    private readonly SqliteConnection _connection;
    private readonly ComicsContext _context;
    private readonly PowerRepository _repository;

    public PowerRepositoryTests()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();
        var builder = new DbContextOptionsBuilder<ComicsContext>();
        builder.UseSqlite(_connection);
        var context = new ComicsContext(builder.Options);
        context.Database.EnsureCreated();
        var flight = new PowerEntity { Id = 1, Name = "flight" };
        var invulnerability = new PowerEntity { Id = 2, Name = "invulnerability" };
        var combatStrategy = new PowerEntity { Id = 3, Name = "combat strategy" };
        context.Powers.AddRange(flight, invulnerability, combatStrategy);
        context.Characters.Add(new CharacterEntity { Id = 1, AlterEgo = "Superman", Powers = new[] { flight, invulnerability } });
        context.SaveChanges();

        _context = context;
        _repository = new PowerRepository(_context, new PowerValidator());
    }

    [Fact]
    public async Task CreateAsync()
    {
        var result = await _repository.CreateAsync(new Power { Name = "super speed" });

        var created = result.Result as Created<Power>;

        created!.Value.Should().Be(new Power { Id = 4, Name = "super speed" });
        created.Location.Should().Be("/powers/4");

        var entity = await _context.Powers.FindAsync(4);
        entity!.Name.Should().Be("super speed");
    }

    [Fact]
    public async Task CreateAsync_Conflict()
    {
        var result = await _repository.CreateAsync(new Power { Name = "invulnerability" });

        var conflict = result.Result as Conflict<Power>;

        conflict!.Value.Should().Be(new Power { Id = 2, Name = "invulnerability" });
    }

    [Fact]
    public async Task CreateAsync_ValidationProblem()
    {
        var result = await _repository.CreateAsync(new Power { Name = string.Empty });

        var validationProblem = result.Result as ValidationProblem;

        validationProblem!.ProblemDetails.Errors.Should().HaveCount(1);
    }

    [Fact]
    public async Task FindAsync_NotFound()
    {
        var result = await _repository.FindAsync(42);

        var notFound = result.Result as NotFound<int>;

        notFound!.Value.Should().Be(42);
    }

    [Fact]
    public async Task FindAsync()
    {
        var result = await _repository.FindAsync(2);

        var ok = result.Result as Ok<Power>;

        ok!.Value.Should().Be(new Power { Id = 2, Name = "invulnerability" });
    }

    [Fact]
    public async Task ReadAsync()
    {
        var powers = await _repository.ReadAsync();

        powers.Should().BeEquivalentTo(new[] {
            new Power { Id = 1, Name = "flight" },
            new Power { Id = 2, Name = "invulnerability" },
            new Power { Id = 3, Name = "combat strategy" }
        });
    }

    [Fact]
    public async Task UpdateAsync()
    {
        var result = await _repository.UpdateAsync(2, new Power { Id = 2, Name = "brilliant deductive skill" });

        result.Result.Should().BeOfType<NoContent>();

        var entity = await _context.Powers.FindAsync(2)!;

        entity!.Name.Should().Be("brilliant deductive skill");
    }

    [Fact]
    public async Task UpdateAsync_NotFound()
    {
        var result = await _repository.UpdateAsync(42, new Power { Id = 42, Name = "brilliant deductive skill" });

        var notFound = result.Result as NotFound<int>;

        notFound!.Value.Should().Be(42);
    }

    [Fact]
    public async Task UpdateAsync_Conflict()
    {
        var result = await _repository.UpdateAsync(2, new Power { Id = 2, Name = "flight" });

        var conflict = result.Result as Conflict<Power>;

        conflict!.Value.Should().Be(new Power { Id = 1, Name = "flight" });

        var entity = _context.Powers.Find(2)!;

        entity.Name.Should().Be("invulnerability");
    }

    [Fact]
    public async Task UpdateAsync_ValidationProblem()
    {
        var result = await _repository.UpdateAsync(2, new Power { Id = 2, Name = string.Empty });

        var validationProblem = result.Result as ValidationProblem;

        validationProblem!.ProblemDetails.Errors.Should().HaveCount(1);
    }

    [Fact]
    public async Task DeleteAsync_NotFound()
    {
        var result = await _repository.DeleteAsync(42);

        var notFound = result.Result as NotFound<int>;

        notFound!.Value.Should().Be(42);
    }

    [Fact]
    public async Task DeleteAsync()
    {
        var result = await _repository.DeleteAsync(3);

        result.Result.Should().BeOfType<NoContent>();

        var entity = await _context.Powers.FindAsync(3);

        entity.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_Conflict()
    {
        var result = await _repository.DeleteAsync(1);

        var conflict = result.Result as Conflict<Power>;

        conflict!.Value.Should().Be(new Power { Id = 1, Name = "flight" });

        (await _context.Powers.FindAsync(1)).Should().NotBeNull();
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
        await _connection.DisposeAsync();
    }
}