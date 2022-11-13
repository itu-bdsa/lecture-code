namespace MyApp.Core.Tests;

public sealed class PowerValidatorTests
{
    private readonly PowerValidator _validator = new PowerValidator();

    [Theory]
    [InlineData("exceptional martial artist")]
    [InlineData("combat strategy")]
    [InlineData("inexhaustible wealth")]
    public void Power_given_valid_name_is_valid(string name)
    {
        var power = new Power { Name = name };

        var result = _validator.TestValidate(power);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("  ")]
    [InlineData("cdbc3e1b-0dfb-4a3a-853c-d26647c2f7be-529c0491-69ad-4970-90d7-8fa4cddb2556")]
    public void Power_given_invalid_name_is_not_valid(string name)
    {
        var power = new Power { Name = name };

        var result = _validator.TestValidate(power);

        result.ShouldHaveValidationErrorFor(power => power.Name);
    }
}
