using System.ComponentModel.DataAnnotations;

namespace MyApp.Core.Tests;

public class CharacterValidationTests
{
    private CharacterCreateDto _batman = new CharacterCreateDto("Batman", "Bruce", "Wayne", 1939, "CEO of Wayne Enterprises", "Gotham City", Male, null, new HashSet<string> { "brilliant deductive skill" });

    [Theory]
    [InlineData("Batman", "Bruce", "Wayne")]
    [InlineData("Batman", null, null)]
    [InlineData(null, "Bruce", null)]
    [InlineData(null, null, "Wayne")]
    public void ValidateName_given_name_returns_Success(string? alterEgo, string? givenName, string? surname)
    {
        var batman = _batman with { AlterEgo = alterEgo, GivenName = givenName, Surname = surname };

        var context = new ValidationContext(batman);

        CharacterValidation.ValidateName(alterEgo, context).Should().Be(ValidationResult.Success);
    }

    [Theory]
    [InlineData(null, null, null)]
    [InlineData("", "", "")]
    [InlineData(" ", " ", " ")]
    public void ValidateName_given_no_name_returns_error(string? alterEgo, string? givenName, string? surname)
    {
        var batman = _batman with { AlterEgo = alterEgo, GivenName = givenName, Surname = surname };

        var context = new ValidationContext(batman);

        var result = CharacterValidation.ValidateName(alterEgo, context)!;

        result.ErrorMessage.Should().Be("Alter ego, given name, or surname must be supplied.");

        result.MemberNames.Single().Should().Be(nameof(CharacterDetailsDto.AlterEgo));
    }

    [Fact]
    public void ValidatePowers_given_valid_power_returns_Success() => CharacterValidation.ValidatePowers(_batman.Powers).Should().Be(ValidationResult.Success);

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void ValidatePowers_given_invalid_power_returns_error(string power)
    {
        var result = CharacterValidation.ValidatePowers(new HashSet<string> { power })!;

        result.ErrorMessage.Should().Be("A power must be between 1 and 50 characters.");
    }
}
