namespace MyApp.Infrastructure.Tests;

public sealed class CityRepositoryTests : IDisposable
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
    public void Create()
    {
        var (response, created) = _repository.Create(new CityCreateDto("Central City"));

        response.Should().Be(Created);

        created.Should().Be(new CityDto(3, "Central City"));
    }

    [Fact]
    public void Create_Conflict()
    {
        var (response, city) = _repository.Create(new CityCreateDto("Gotham City"));

        response.Should().Be(Conflict);

        city.Should().Be(new CityDto(2, "Gotham City"));
    }

    [Fact]
    public void Find() => _repository.Find(2).Should().Be(new CityDto(2, "Gotham City"));

    [Fact]
    public void Find_Non_Existing() => _repository.Find(42).Should().BeNull();

    [Fact]
    public void Read() => _repository.Read().Should().BeEquivalentTo(new[] { new CityDto(1, "Metropolis"), new CityDto(2, "Gotham City") });

    [Fact]
    public void Update_Non_Existing() => _repository.Update(new CityDto(42, "Central City")).Should().Be(NotFound);

    [Fact]
    public void Update_Conflict()
    {
        var response = _repository.Update(new CityDto(2, "Metropolis"));

        response.Should().Be(Conflict);

        var entity = _context.Cities.Find(2)!;

        entity.Name.Should().Be("Gotham City");
    }

    [Fact]
    public void Update()
    {
        var response = _repository.Update(new CityDto(2, "Central City"));

        response.Should().Be(Updated);

        var entity = _context.Cities.Find(2)!;

        entity.Name.Should().Be("Central City");
    }

    [Fact]
    public void Delete_Non_Existing() => _repository.Delete(42).Should().Be(NotFound);

    [Fact]
    public void Delete()
    {
        var response = _repository.Delete(2);

        response.Should().Be(Deleted);

        var entity = _context.Cities.Find(2);

        entity.Should().BeNull();
    }

    [Fact]
    public void Delete_Conflict()
    {
        var response = _repository.Delete(1);

        response.Should().Be(Conflict);

        _context.Cities.Find(1).Should().NotBeNull();
    }

    public void Dispose()
    {
        _context.Dispose();
        _connection.Dispose();
    }
}
