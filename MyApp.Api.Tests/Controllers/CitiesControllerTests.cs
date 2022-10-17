namespace MyApp.Api.Tests;

public class CitiesControllerTests
{
    [Fact]
    public void Get_given_repo_returns_null_returns_NotFound()
    {
        var repository = Substitute.For<CityRepository>();

    }
}