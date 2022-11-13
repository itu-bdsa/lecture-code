namespace MyApp.Core.Tests;

public sealed class CityValidatorTests
{
    private readonly CityValidator _validator = new CityValidator();

    [Theory]
    [InlineData("Metropolis")]
    [InlineData("Gotham City")]
    [InlineData("Themyscira")]
    public void City_given_valid_name_is_valid(string name)
    {
        var city = new City { Name = name };

        var result = _validator.TestValidate(city);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("  ")]
    [InlineData("cdbc3e1b-0dfb-4a3a-853c-d26647c2f7be-529c0491-69ad-4970-90d7-8fa4cddb2556")]
    public void City_given_invalid_name_is_not_valid(string name)
    {
        var city = new City { Name = name };

        var result = _validator.TestValidate(city);

        result.ShouldHaveValidationErrorFor(city => city.Name);
    }
}
