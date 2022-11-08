namespace MyApp.Integration.Tests;

public class CharactersEndpointTests : IClassFixture<IntegrationTestFactory>
{
    private readonly IntegrationTestFactory _factory;

    public CharactersEndpointTests(IntegrationTestFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetOne()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var character = await client.GetFromJsonAsync<CharacterDetailsDto>("/characters/3");

        // Assert
        character!.AlterEgo.Should().Be("Wonder Woman");
    }

    [Fact]
    public async Task GetAll()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var characters = await client.GetFromJsonAsync<CharacterDto[]>("/characters");

        // Assert
        characters.Should().Contain(new CharacterDto(3, "Wonder Woman", "Diana", "Prince"));
    }

    [Fact]
    public async Task Post()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var harleyQuinn = new CharacterCreateDto(
            AlterEgo: "Harley Quinn",
            GivenName: "Harleen",
            Surname: "Quinzel",
            FirstAppearance: 1992,
            Occupation: "Former psychiatrist",
            City: "Gotham City",
            Gender: Female,
            ImageUrl: "https://upload.wikimedia.org/wikipedia/en/a/ab/Harley_Quinn_Infobox.png",
            Powers: new HashSet<string> { "complete unpredictability", "superhuman agility", "skilled fighter", "intelligence", "emotional manipulation", "immunity to toxins" }
        );
        var response = await client.PostAsJsonAsync("/characters", harleyQuinn);
        var character = await response.Content.ReadFromJsonAsync<CharacterDetailsDto>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        character.Should().BeEquivalentTo(new CharacterDetailsDto(
            Id: 5,
            AlterEgo: "Harley Quinn",
            GivenName: "Harleen",
            Surname: "Quinzel",
            FirstAppearance: 1992,
            Occupation: "Former psychiatrist",
            City: "Gotham City",
            Gender: Female,
            ImageUrl: "https://upload.wikimedia.org/wikipedia/en/a/ab/Harley_Quinn_Infobox.png",
            Powers: new HashSet<string> { "complete unpredictability", "superhuman agility", "skilled fighter", "intelligence", "emotional manipulation", "immunity to toxins" }
        ));
    }

    [Fact]
    public async Task Put()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var riddler = new CharacterUpdateDto(
            Id: 2,
            AlterEgo: "Riddler",
            GivenName: "Edward",
            Surname: "Nygma",
            FirstAppearance: 1948,
            Occupation: "Professional criminal",
            City: "Gotham City",
            Gender: Male,
            ImageUrl: "https://upload.wikimedia.org/wikipedia/en/6/68/Riddler.png",
            Powers: new HashSet<string> { "genius-level intellect" }
        );
        var response = await client.PutAsJsonAsync("/characters/2", riddler);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Delete()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.DeleteAsync("/characters/1");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}