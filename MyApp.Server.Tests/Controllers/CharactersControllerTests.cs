namespace MyApp.Server.Tests;

public class CharactersControllerTests
{
    private readonly ICharacterRepository _repository;
    private readonly CharactersController _sut;

    public CharactersControllerTests()
    {
        var logger = Substitute.For<ILogger<CharactersController>>();
        _repository = Substitute.For<ICharacterRepository>();
        _sut = new CharactersController(logger, _repository);
    }

    [Fact]
    public async Task Get_NonExisting() => (await _sut.Get(42)).Result.Should().BeAssignableTo<NotFoundResult>();

    [Fact]
    public async Task Get_Existing()
    {
        var character = new CharacterDetailsDto(3, "Batman", "Bruce", "Wayne", 1939, "CEO of Wayne Enterprises", "Gotham City", Male, "https://images.com/batman.png", new HashSet<string> { "exceptional martial artist", "combat strategy", "inexhaustible wealth", "brilliant deductive skill", "advanced technology" });
        _repository.FindAsync(3).Returns(character);

        var response = await _sut.Get(3);

        response.Value.Should().Be(character);
    }

    [Fact]
    public async Task Get_All()
    {
        var cities = new CharacterDto[] { new(1, "Superman", "Clark", "Kent"), new(2, "Batman", "Bruce", "Wayne") };

        _repository.ReadAsync().Returns(cities);

        var response = await _sut.Get();

        response.Should().BeEquivalentTo(cities);
    }

    [Fact]
    public async Task Post()
    {
        var create = new CharacterCreateDto("Wonder Woman", "Diana", null, 1941, "Amazon Princess", "Themyscira", Female, "https://images.com/wonder-woman.png", new HashSet<string> { "super strength", "invulnerability", "flight", "combat skill", "combat strategy", "superhuman agility", "healing factor", "magic weaponry" });
        var created = new CharacterDetailsDto(4, "Wonder Woman", "Diana", null, 1941, "Amazon Princess", "Themyscira", Female, "https://images.com/wonder-woman.png", new HashSet<string> { "super strength", "invulnerability", "flight", "combat skill", "combat strategy", "superhuman agility", "healing factor", "magic weaponry" });

        _repository.CreateAsync(create).Returns(created);

        var response = await _sut.Post(create);
        var result = response as CreatedAtActionResult;

        result!.ActionName.Should().Be(nameof(CharactersController.Get));
        result.RouteValues!.Single().Should().Be(new KeyValuePair<string, object>("id", 4));
        result.Value.Should().Be(created);
    }

    [Fact]
    public async Task Put()
    {
        var update = new CharacterUpdateDto(3, "Batman", "Bruce", "Wayne", 1939, "CEO of Wayne Enterprises", "Gotham City", Male, "https://images.com/batman.png", new HashSet<string> { "exceptional martial artist", "combat strategy", "inexhaustible wealth", "brilliant deductive skill", "advanced technology" });

        _repository.UpdateAsync(update).Returns(Status.Updated);

        var response = await _sut.Put(3, update);

        response.Should().BeAssignableTo<NoContentResult>();
    }

    [Fact]
    public async Task Put_NonExisting()
    {
        var update = new CharacterUpdateDto(42, "Wonder Woman", "Diana", null, 1941, "Amazon Princess", "Themyscira", Female, "https://images.com/wonder-woman.png", new HashSet<string> { "super strength", "invulnerability", "flight", "combat skill", "combat strategy", "superhuman agility", "healing factor", "magic weaponry" });

        _repository.UpdateAsync(update).Returns(Status.NotFound);

        var response = await _sut.Put(42, update);

        response.Should().BeAssignableTo<NotFoundResult>();
    }

    [Fact]
    public async Task Delete_Existing()
    {
        _repository.DeleteAsync(1).Returns(Status.Deleted);

        var response = await _sut.Delete(1);

        response.Should().BeAssignableTo<NoContentResult>();
    }

    [Fact]
    public async Task Delete_NonExisting()
    {
        _repository.DeleteAsync(42).Returns(Status.NotFound);

        var response = await _sut.Delete(42);

        response.Should().BeAssignableTo<NotFoundResult>();
    }
}