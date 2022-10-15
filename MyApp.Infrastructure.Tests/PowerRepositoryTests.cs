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
        var flight = new Power("flight") { Id = 1 };
        var invulnerability = new Power("invulnerability") { Id = 2 };
        var combatStrategy = new Power("combat strategy") { Id = 3 };
        context.Powers.AddRange(flight, invulnerability, combatStrategy);
        context.Characters.Add(new Character { Id = 1, AlterEgo = "Superman", Powers = new[] { flight, invulnerability } });
        context.SaveChanges();

        _context = context;
        _repository = new PowerRepository(_context);
    }

    [Fact]
    public async Task Create()
    {
        var (response, created) = await _repository.CreateAsync(new PowerCreateDto("super speed"));

        response.Should().Be(Created);

        created.Should().Be(new PowerDto(4, "super speed"));
    }

    [Fact]
    public async Task Create_Conflict()
    {
        var (response, power) = await _repository.CreateAsync(new PowerCreateDto("invulnerability"));

        response.Should().Be(Conflict);

        power.Should().Be(new PowerDto(2, "invulnerability"));
    }

    [Fact]
    public async Task Find_Non_Existing() => (await _repository.FindAsync(42)).Should().BeNull();

    [Fact]
    public async Task Find() => (await _repository.FindAsync(2)).Should().Be(new PowerDto(2, "invulnerability"));

    [Fact]
    public async Task Read() => (await _repository.ReadAsync()).Should().BeEquivalentTo(new[] { new PowerDto(1, "flight"), new PowerDto(2, "invulnerability"), new PowerDto(3, "combat strategy") });

    [Fact]
    public async Task Update_Non_Existing() => (await _repository.UpdateAsync(new PowerDto(42, "brilliant deductive skill"))).Should().Be(NotFound);

    [Fact]
    public async Task Update_Conflict()
    {
        var response = await _repository.UpdateAsync(new PowerDto(2, "flight"));

        response.Should().Be(Conflict);

        var entity = _context.Powers.Find(2)!;

        entity.Name.Should().Be("invulnerability");
    }

    [Fact]
    public async Task Update()
    {
        var response = await _repository.UpdateAsync(new PowerDto(2, "brilliant deductive skill"));

        response.Should().Be(Updated);

        var entity = await _context.Powers.FindAsync(2)!;

        entity!.Name.Should().Be("brilliant deductive skill");
    }

    [Fact]
    public async Task Delete_Non_Existing() => (await _repository.DeleteAsync(42)).Should().Be(NotFound);

    [Fact]
    public async Task Delete()
    {
        var response = await _repository.DeleteAsync(3);

        response.Should().Be(Deleted);

        var entity = await _context.Powers.FindAsync(3);

        entity.Should().BeNull();
    }

    [Fact]
    public async Task Delete_Conflict()
    {
        var response = await _repository.DeleteAsync(1);

        response.Should().Be(Conflict);

        (await _context.Powers.FindAsync(1)).Should().NotBeNull();
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
        await _connection.DisposeAsync();
    }
}