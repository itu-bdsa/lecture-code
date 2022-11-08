namespace MyApp.Integration.Tests;

public class PowersEndpointTests : IClassFixture<IntegrationTestFactory>
{
    private readonly IntegrationTestFactory _factory;

    public PowersEndpointTests(IntegrationTestFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetOne()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var power = await client.GetFromJsonAsync<PowerDto>("/powers/10");

        // Assert
        power.Should().Be(new PowerDto(10, "exceptional martial artist"));
    }

    [Fact]
    public async Task GetAll()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var powers = await client.GetFromJsonAsync<PowerDto[]>("/powers");

        // Assert
        powers.Should().Contain(new PowerDto(10, "exceptional martial artist"));
    }

    [Fact]
    public async Task Post()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync("/powers", new PowerCreateDto("genius-level intellect"));
        var power = await response.Content.ReadFromJsonAsync<PowerDto>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        power.Should().Be(new PowerDto(19, "genius-level intellect"));
    }

    [Fact]
    public async Task Put()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.PutAsJsonAsync("/powers/2", new PowerDto(2, "ability to breathe underwater"));

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Delete()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.DeleteAsync("/powers/1");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }
}