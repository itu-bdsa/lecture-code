namespace Generics.Tests;

public sealed class PostalCodeValidatorTests
{
    [Theory]
    [InlineData("2000", true)]
    [InlineData("2720", true)]
    [InlineData("0000", true)]
    [InlineData("  2000  ", false)]
    [InlineData("0", false)]
    [InlineData("fish", false)]
    public void IsValid_given_input_returns_valid(string input, bool valid)
    {
        PostalCodeValidator.IsValid(input).Should().Be(valid);
    }

    [Theory]
    [InlineData("2000 Frederiksberg C", true, "2000", "Frederiksberg C")]
    [InlineData("9210 Aalborg SØ", true, "9210", "Aalborg SØ")]
    [InlineData("fish", false, "", "")]
    [InlineData("100 Tórshavn", false, "", "")]
    public void TryParse_given_input_returns_expectedValid_expectedPostalCode_and_expectedLocality(
            string input,
            bool expectedValid,
            string expectedPostalCode,
            string expectedLocality)
    {
        var valid = PostalCodeValidator.TryParse(input, out string postalCode, out string locality);

        valid.Should().Be(expectedValid);
        postalCode.Should().Be(expectedPostalCode);
        locality.Should().Be(expectedLocality);
    }

    [Theory]
    [InlineData("2000 Frederiksberg C", true, "2000", "Frederiksberg C")]
    [InlineData("9210 Aalborg SØ", true, "9210", "Aalborg SØ")]
    [InlineData("fish", false, "", "")]
    [InlineData("100 Tórshavn", false, "", "")]
    public void TryParseTupled_given_input_returns_expectedValid_expectedPostalCode_and_expectedLocality(
        string input,
        bool expectedValid,
        string expectedPostalCode,
        string expectedLocality)
    {
        var (valid, postalCode, locality) = PostalCodeValidator.TryParse(input);

        (valid, postalCode, locality).Should().Be((expectedValid, expectedPostalCode, expectedLocality));
    }
}