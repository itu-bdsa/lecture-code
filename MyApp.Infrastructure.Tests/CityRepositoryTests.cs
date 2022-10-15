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
        context.Cities.AddRange(new City("Metropolis") { Id = 1 }, new City("Gotham City") { Id = 2 });
        context.Characters.Add(new Character { Id = 1, AlterEgo = "Superman", CityId = 1 });
        context.SaveChanges();

        _context = context;
        _repository = new CityRepository(_context);
    }

    [Fact]
    public async Task Create()
    {
        var (response, created) = await _repository.CreateAsync(new CityCreateDto("Central City"));

        response.Should().Be(Created);

        created.Should().Be(new CityDto(3, "Central City"));
    }

    [Fact]
    public async Task Create_Conflict()
    {
        var (response, city) = await _repository.CreateAsync(new CityCreateDto("Gotham City"));

        response.Should().Be(Conflict);

        city.Should().Be(new CityDto(2, "Gotham City"));
    }

    [Fact]
    public async Task Find() => (await _repository.FindAsync(2)).Should().Be(new CityDto(2, "Gotham City"));

    [Fact]
    public async Task Find_Non_Existing() => (await _repository.FindAsync(42)).Should().BeNull();

    [Fact]
    public async Task Read() => (await _repository.ReadAsync()).Should().BeEquivalentTo(new[] { new CityDto(1, "Metropolis"), new CityDto(2, "Gotham City") });

    [Fact]
    public async Task Update_Non_Existing() => (await _repository.UpdateAsync(new CityDto(42, "Central City"))).Should().Be(NotFound);

    [Fact]
    public async Task Update_Conflict()
    {
        var response = await _repository.UpdateAsync(new CityDto(2, "Metropolis"));

        response.Should().Be(Conflict);

        var entity = await _context.Cities.FindAsync(2);

        entity!.Name.Should().Be("Gotham City");
    }

    [Fact]
    public async Task Update()
    {
        var response = await _repository.UpdateAsync(new CityDto(2, "Central City"));

        response.Should().Be(Updated);

        var entity = await _context.Cities.FindAsync(2)!;

        entity!.Name.Should().Be("Central City");
    }

    [Fact]
    public async Task Delete_Non_Existing() => (await _repository.DeleteAsync(42)).Should().Be(NotFound);

    [Fact]
    public async Task Delete()
    {
        var response = await _repository.DeleteAsync(2);

        response.Should().Be(Deleted);

        var entity = await _context.Cities.FindAsync(2);

        entity.Should().BeNull();
    }

    [Fact]
    public async Task Delete_Conflict()
    {
        var response = await _repository.DeleteAsync(1);

        response.Should().Be(Conflict);

        (await _context.Cities.FindAsync(1)).Should().NotBeNull();
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
        await _connection.DisposeAsync();
    }
}
