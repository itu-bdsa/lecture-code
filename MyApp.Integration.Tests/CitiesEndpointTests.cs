namespace MyApp.Integration.Tests;

public class CitiesEndpointTests : IClassFixture<IntegrationTestFactory>
{
    private readonly IntegrationTestFactory _factory;

    public CitiesEndpointTests(IntegrationTestFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetOne()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var city = await client.GetFromJsonAsync<CityDto>("/cities/2");

        // Assert
        city.Should().Be(new CityDto(2, "Gotham City"));
    }

    [Fact]
    public async Task GetAll()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var cities = await client.GetFromJsonAsync<CityDto[]>("/cities");

        // Assert
        cities.Should().Contain(new CityDto(2, "Gotham City"));
    }

    [Fact]
    public async Task Post()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync("/cities", new CityCreateDto("Central City"));
        var city = await response.Content.ReadFromJsonAsync<CityDto>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        city.Should().Be(new CityDto(4, "Central City"));
    }

    [Fact]
    public async Task Put()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.PutAsJsonAsync("/cities/2", new CityDto(2, "Coast City"));

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Delete()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.DeleteAsync("/cities/1");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }
}