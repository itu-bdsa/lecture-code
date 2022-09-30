namespace MyApp.Infrastructure.Tests;

public sealed class PowerRepositoryTests : IDisposable
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
    public void Create()
    {
        var (response, created) = _repository.Create(new PowerCreateDto("super speed"));

        response.Should().Be(Created);

        created.Should().Be(new PowerDto(4, "super speed"));
    }

    [Fact]
    public void Create_Conflict()
    {
        var (response, power) = _repository.Create(new PowerCreateDto("invulnerability"));

        response.Should().Be(Conflict);

        power.Should().Be(new PowerDto(2, "invulnerability"));
    }

    [Fact]
    public void Find_Non_Existing() => _repository.Find(42).Should().BeNull();

    [Fact]
    public void Find() => _repository.Find(2).Should().Be(new PowerDto(2, "invulnerability"));

    [Fact]
    public void Read() => _repository.Read().Should().BeEquivalentTo(new[] { new PowerDto(1, "flight"), new PowerDto(2, "invulnerability"), new PowerDto(3, "combat strategy") });

    [Fact]
    public void Update_Non_Existing() => _repository.Update(new PowerDto(42, "brilliant deductive skill")).Should().Be(NotFound);

    [Fact]
    public void Update_Conflict()
    {
        var response = _repository.Update(new PowerDto(2, "flight"));

        response.Should().Be(Conflict);

        var entity = _context.Powers.Find(2)!;

        entity.Name.Should().Be("invulnerability");
    }

    [Fact]
    public void Update()
    {
        var response = _repository.Update(new PowerDto(2, "brilliant deductive skill"));

        response.Should().Be(Updated);

        var entity = _context.Powers.Find(2)!;

        entity.Name.Should().Be("brilliant deductive skill");
    }

    [Fact]
    public void Delete_Non_Existing() => _repository.Delete(42).Should().Be(NotFound);

    [Fact]
    public void Delete()
    {
        var response = _repository.Delete(3);

        response.Should().Be(Deleted);

        var entity = _context.Powers.Find(3);

        entity.Should().BeNull();
    }

    [Fact]
    public void Delete_Conflict()
    {
        var response = _repository.Delete(1);

        response.Should().Be(Conflict);

        _context.Powers.Find(1).Should().NotBeNull();
    }


    public void Dispose()
    {
        _context.Dispose();
        _connection.Dispose();
    }
}