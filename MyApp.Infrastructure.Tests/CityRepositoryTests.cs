namespace MyApp.Infrastructure.Tests;

public sealed class CityRepositoryTests : IAsyncDisposable
{
    private readonly SqliteConnection _connection;
    private readonly ComicsContext _context;
    private readonly CityRepository _repository;

    public CityRepositoryTests()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();
        var builder = new DbContextOptionsBuilder<ComicsContext>();
        builder.UseSqlite(_connection);
        var context = new ComicsContext(builder.Options);
        context.Database.EnsureCreated();
        var metropolis = new CityEntity { Name = "Metropolis" };
        var gothamCity = new CityEntity { Name = "Gotham City" };
        var themyscira = new CityEntity { Name = "Themyscira" };
        context.Cities.AddRange(metropolis, gothamCity, themyscira);
        context.Characters.Add(new CharacterEntity { Id = 1, AlterEgo = "Superman", City = metropolis });
        context.SaveChanges();

        _context = context;
        _repository = new CityRepository(_context, new CityValidator());
    }

    [Fact]
    public async Task CreateAsync()
    {
        var result = await _repository.CreateAsync(new City { Name = "Central City" });

        var created = result.Result as Created<City>;

        created!.Value.Should().Be(new City { Id = 4, Name = "Central City" });
        created.Location.Should().Be("/cities/4");

        var entity = await _context.Cities.FindAsync(4);
        entity!.Name.Should().Be("Central City");
    }

    [Fact]
    public async Task CreateAsync_Conflict()
    {
        var result = await _repository.CreateAsync(new City { Name = "Gotham City" });

        var conflict = result.Result as Conflict<City>;

        conflict!.Value.Should().Be(new City { Id = 2, Name = "Gotham City" });
    }

    [Fact]
    public async Task CreateAsync_ValidationProblem()
    {
        var result = await _repository.CreateAsync(new City { Name = string.Empty });

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

        var ok = result.Result as Ok<City>;

        ok!.Value.Should().Be(new City { Id = 2, Name = "Gotham City" });
    }

    [Fact]
    public async Task ReadAsync()
    {
        var powers = await _repository.ReadAsync();

        powers.Should().BeEquivalentTo(new[] {
            new City { Id = 1, Name = "Metropolis" },
            new City { Id = 2, Name = "Gotham City" },
            new City { Id = 3, Name = "Themyscira" }
        });
    }

    [Fact]
    public async Task UpdateAsync()
    {
        var result = await _repository.UpdateAsync(2, new City { Id = 2, Name = "Central City" });

        result.Result.Should().BeOfType<NoContent>();

        var entity = await _context.Cities.FindAsync(2)!;

        entity!.Name.Should().Be("Central City");
    }

    [Fact]
    public async Task UpdateAsync_NotFound()
    {
        var result = await _repository.UpdateAsync(42, new City { Id = 42, Name = "Central City" });

        var notFound = result.Result as NotFound<int>;

        notFound!.Value.Should().Be(42);
    }

    [Fact]
    public async Task UpdateAsync_Conflict()
    {
        var result = await _repository.UpdateAsync(2, new City { Id = 2, Name = "Metropolis" });

        var conflict = result.Result as Conflict<City>;

        conflict!.Value.Should().Be(new City { Id = 1, Name = "Metropolis" });

        var entity = _context.Cities.Find(2)!;

        entity.Name.Should().Be("Gotham City");
    }

    [Fact]
    public async Task UpdateAsync_ValidationProblem()
    {
        var result = await _repository.UpdateAsync(2, new City { Id = 2, Name = string.Empty });

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

        var entity = await _context.Cities.FindAsync(3);

        entity.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_Conflict()
    {
        var result = await _repository.DeleteAsync(1);

        var conflict = result.Result as Conflict<City>;

        conflict!.Value.Should().Be(new City { Id = 1, Name = "Metropolis" });

        (await _context.Cities.FindAsync(1)).Should().NotBeNull();
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
        await _connection.DisposeAsync();
    }
}