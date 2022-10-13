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

        var superman = new Character
        {
            AlterEgo = "Superman",
            GivenName = "Clark",
            Surname = "Kent",
            FirstAppearance = 1938,
            Occupation = "Reporter",
            City = new City("Metropolis"),
            Gender = Male,
            ImageUrl = "https://images.com/catwoman.png",
            Powers = new Power[] { new("super strength"), new("flight"), new("invulnerability"), new("super speed"), new("heat vision"), new("freeze breath"), new("x-ray vision"), new("superhuman hearing"), new("healing factor") }
        };

        var catwoman = new Character
        {
            AlterEgo = "Catwoman",
            GivenName = "Selina",
            Surname = "Kyle",
            FirstAppearance = 1940,
            Occupation = "Thief",
            City = new City("Gotham City"),
            Gender = Female,
            ImageUrl = "https://images.com/catwoman.png",
            Powers = new Power[] { new("exceptional martial artist"), new("gymnastic ability"), new("combat skill") }
        };

        _context.Characters.AddRange(superman, catwoman);
        _context.SaveChanges();

        _repository = new CharacterRepository(_context);
    }

    [Fact]
    public async Task Create()
    {
        var character = new CharacterCreateDto("Batman", "Bruce", "Wayne", 1939, "CEO of Wayne Enterprises", "Gotham City", Male, "https://images.com/batman.png", new HashSet<string> { "exceptional martial artist", "combat strategy", "inexhaustible wealth", "brilliant deductive skill", "advanced technology" });

        var expected = new CharacterDetailsDto(3, "Batman", "Bruce", "Wayne", 1939, "CEO of Wayne Enterprises", "Gotham City", Male, "https://images.com/batman.png", new HashSet<string> { "exceptional martial artist", "combat strategy", "inexhaustible wealth", "brilliant deductive skill", "advanced technology" });

        var (status, created) = await _repository.CreateAsync(character);

        status.Should().Be(Created);
        created.Should().BeEquivalentTo(expected);
        (await _repository.FindAsync(3)).Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task Find() => (await _repository.FindAsync(2)).Should().BeEquivalentTo(new CharacterDetailsDto(2, "Catwoman", "Selina", "Kyle", 1940, "Thief", "Gotham City", Female, "https://images.com/catwoman.png", new HashSet<string> { "exceptional martial artist", "gymnastic ability", "combat skill" }));

    [Fact]
    public async Task Find_Non_Existing() => (await _repository.FindAsync(42)).Should().BeNull();

    [Fact]
    public async Task Read()
    {
        var characters = await _repository.Read();

        var expected = new CharacterDto[] {
            new(1, "Superman", "Clark", "Kent"),
            new(2, "Catwoman", "Selina", "Kyle")
        };

        characters.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task Update()
    {
        var character = new CharacterUpdateDto(1, "Wonder Woman", "Diana", null, 1941, "Amazon Princess", "Themyscira", Female, "https://images.com/wonder-woman.png", new HashSet<string> { "super strength", "invulnerability", "flight", "combat skill", "combat strategy", "superhuman agility", "healing factor", "magic weaponry" });

        var expected = new CharacterDetailsDto(1, "Wonder Woman", "Diana", null, 1941, "Amazon Princess", "Themyscira", Female, "https://images.com/wonder-woman.png", new HashSet<string> { "super strength", "invulnerability", "flight", "combat skill", "combat strategy", "superhuman agility", "healing factor", "magic weaponry" });

        var status = await _repository.UpdateAsync(character);

        status.Should().Be(Updated);
        (await _repository.FindAsync(1)).Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task Update_Non_Existing()
    {
        var character = new CharacterUpdateDto(42, "Wonder Woman", "Diana", null, 1941, "Amazon Princess", "Themyscira", Female, "https://images.com/wonder-woman.png", new HashSet<string> { "super strength", "invulnerability", "flight", "combat skill", "combat strategy", "superhuman agility", "healing factor", "magic weaponry" });

        var status = await _repository.UpdateAsync(character);

        status.Should().Be(NotFound);
    }

    [Fact]
    public async Task Delete()
    {
        var status = await _repository.DeleteAsync(1);

        status.Should().Be(Deleted);
        (await _context.Characters.FindAsync(1)).Should().BeNull();
    }

    [Fact]
    public async Task Delete_Non_Existing() => (await _repository.DeleteAsync(42)).Should().Be(NotFound);

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
        await _connection.DisposeAsync();
    }
}