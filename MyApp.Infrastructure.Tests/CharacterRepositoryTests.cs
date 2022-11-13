namespace MyApp.Infrastructure.Tests;

public sealed class CharacterRepositoryTests : IAsyncDisposable
{
    private readonly SqliteConnection _connection;
    private readonly ComicsContext _context;
    private readonly CharacterRepository _repository;

    public CharacterRepositoryTests()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();
        var builder = new DbContextOptionsBuilder<ComicsContext>().UseSqlite(_connection);
        _context = new ComicsContext(builder.Options);
        _context.Database.EnsureCreated();

        var superman = new CharacterEntity
        {
            AlterEgo = "Superman",
            GivenName = "Clark",
            Surname = "Kent",
            FirstAppearance = 1938,
            Occupation = "Reporter",
            City = new CityEntity { Name = "Metropolis" },
            Gender = Male,
            ImageUrl = "https://images.com/catwoman.png",
            Powers = new HashSet<PowerEntity>
            {
                new PowerEntity { Name = "super strength" },
                new PowerEntity { Name = "flight" },
                new PowerEntity { Name = "invulnerability" },
                new PowerEntity { Name = "super speed" },
                new PowerEntity { Name = "heat vision" },
                new PowerEntity { Name = "freeze breath" },
                new PowerEntity { Name = "x-ray vision" },
                new PowerEntity { Name = "superhuman hearing" },
                new PowerEntity { Name = "healing factor" }
            }
        };

        var catwoman = new CharacterEntity
        {
            AlterEgo = "Catwoman",
            GivenName = "Selina",
            Surname = "Kyle",
            FirstAppearance = 1940,
            Occupation = "Thief",
            City = new CityEntity { Name = "Gotham City" },
            Gender = Female,
            ImageUrl = "https://images.com/catwoman.png",
            Powers = new HashSet<PowerEntity>
            {
                new PowerEntity { Name = "exceptional martial artist" },
                new PowerEntity { Name = "gymnastic ability" },
                new PowerEntity { Name = "combat skill" }
            }
        };

        _context.Characters.AddRange(superman, catwoman);
        _context.SaveChanges();

        _repository = new CharacterRepository(_context, new CharacterValidator());
    }

    [Fact]
    public async Task CreateAsync()
    {
        var character = new Character
        {
            AlterEgo = "Batman",
            GivenName = "Bruce",
            Surname = "Wayne",
            FirstAppearance = 1939,
            Occupation = "CEO of Wayne Enterprises",
            City = "Gotham City",
            Gender = Male,
            ImageUrl = "https://images.com/batman.png",
            Powers = new HashSet<string>
            {
                "exceptional martial artist",
                "combat strategy",
                "inexhaustible wealth",
                "brilliant deductive skill",
                "advanced technology"
            }
        };

        var result = await _repository.CreateAsync(character);

        var created = result.Result as Created<Character>;

        created!.Value.Should().BeEquivalentTo(character with { Id = 3 });
        created.Location.Should().Be("3");

        ((await _repository.FindAsync(3)).Result as Ok<Character>)!.Value.Should().BeEquivalentTo(character with { Id = 3 });
    }

    [Fact]
    public async Task CreateAsync_ValidationProblem()
    {
        var character = new Character();

        var result = await _repository.CreateAsync(character);

        var validationProblem = result.Result as ValidationProblem;

        validationProblem!.ProblemDetails.Errors.Should().HaveCountGreaterThanOrEqualTo(1);
    }

    [Fact]
    public async Task FindAsync()
    {
        var expected = new Character
        {
            Id = 2,
            AlterEgo = "Catwoman",
            GivenName = "Selina",
            Surname = "Kyle",
            FirstAppearance = 1940,
            Occupation = "Thief",
            City = "Gotham City",
            Gender = Female,
            ImageUrl = "https://images.com/catwoman.png",
            Powers = new HashSet<string>
            {
                "exceptional martial artist",
                "gymnastic ability",
                "combat skill"
            }
        };

        var result = await _repository.FindAsync(2);

        var actual = result.Result as Ok<Character>;

        actual!.Value.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task FindAsync_NotFound()
    {
        var result = await _repository.FindAsync(42);

        var notFound = result.Result as NotFound<int>;

        notFound!.Value.Should().Be(42);
    }

    [Fact]
    public async Task ReadAsync()
    {
        var expected = new BasicCharacter[]
        {
            new BasicCharacter { Id = 1, AlterEgo = "Superman", GivenName = "Clark", Surname ="Kent" },
            new BasicCharacter { Id = 2, AlterEgo = "Catwoman", GivenName = "Selina", Surname = "Kyle" }
        };

        var characters = await _repository.ReadAsync();

        characters.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task UpdateAsync()
    {
        var character = new Character
        {
            Id = 1,
            AlterEgo = "Wonder Woman",
            GivenName = "Diana",
            Surname = null,
            FirstAppearance = 1941,
            Occupation = "Amazon Princess",
            City = "Themyscira",
            Gender = Female,
            ImageUrl = "https://images.com/wonder-woman.png",
            Powers = new HashSet<string> { "super strength", "invulnerability", "flight", "combat skill", "combat strategy", "superhuman agility", "healing factor", "magic weaponry" }
        };

        var result = await _repository.UpdateAsync(1, character);

        result.Result.Should().BeOfType<NoContent>();

        ((await _repository.FindAsync(1)).Result as Ok<Character>)!.Value.Should().BeEquivalentTo(character);
    }

    [Fact]
    public async Task UpdateAsync_NotFound()
    {
        var character = new Character { Id = 42, AlterEgo = "Green Lantern" };

        var result = await _repository.UpdateAsync(42, character);

        var notFound = result.Result as NotFound<int>;

        notFound!.Value.Should().Be(42);
    }

    [Fact]
    public async Task DeleteAsync()
    {
        var result = await _repository.DeleteAsync(1);

        result.Result.Should().BeOfType<NoContent>();

        (await _context.Characters.FindAsync(1)).Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_NotFound()
    {
        var result = await _repository.DeleteAsync(42);

        var notFound = result.Result as NotFound<int>;

        notFound!.Value.Should().Be(42);
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
        await _connection.DisposeAsync();
    }
}