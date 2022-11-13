namespace MyApp.Core.Tests;

public sealed class CharacterValidatorTests
{
    private readonly CharacterValidator _validator = new CharacterValidator();
    private readonly Character _character = new Character
    {
        AlterEgo = "Riddler",
        GivenName = "Edward",
        Surname = "Nygma",
        FirstAppearance = 1948,
        Occupation = "Professional criminal",
        City = "Gotham City",
        Gender = Male,
        ImageUrl = "https://upload.wikimedia.org/wikipedia/en/6/68/Riddler.png",
        Powers = new HashSet<string> { "genius-level intellect" }
    };

    [Fact]
    public void Character_given_valid_input_is_valid()
    {
        var result = _validator.TestValidate(_character);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Character_given_only_alterEgo_is_valid()
    {
        var character = new Character { AlterEgo = "Batman" };

        var result = _validator.TestValidate(character);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Character_given_no_name_is_not_valid()
    {
        var character = new Character { AlterEgo = null, GivenName = null, Surname = null };

        var result = _validator.TestValidate(character);

        result.ShouldHaveValidationErrorFor(character => character).WithErrorMessage("At least one of alter ego, given name, or surname must be supplied.");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("  ")]
    [InlineData("cdbc3e1b-0dfb-4a3a-853c-d26647c2f7be-529c0491-69ad-4970-90d7-8fa4cddb2556")]
    public void Character_given_invalid_stringValue_is_not_valid(string stringValue)
    {
        var character = _character with
        {
            AlterEgo = stringValue,
            GivenName = stringValue,
            Surname = stringValue,
            Occupation = stringValue,
            City = stringValue,
        };

        var result = _validator.TestValidate(character);

        result.ShouldHaveValidationErrorFor(character => character.AlterEgo);
        result.ShouldHaveValidationErrorFor(character => character.GivenName);
        result.ShouldHaveValidationErrorFor(character => character.Surname);
        result.ShouldHaveValidationErrorFor(character => character.Occupation);
        result.ShouldHaveValidationErrorFor(character => character.City);
    }

    [Theory]
    [InlineData(null)]
    [InlineData((short)1900)]
    [InlineData((short)2000)]
    [InlineData((short)2100)]
    public void Character_given_valid_firstAppearance_is_valid(short? firstAppearance)
    {
        var character = new Character { FirstAppearance = firstAppearance };

        var result = _validator.TestValidate(character);

        result.ShouldNotHaveValidationErrorFor(character => character.FirstAppearance);
    }

    [Theory]
    [InlineData((short)-32768)]
    [InlineData((short)0)]
    [InlineData((short)1899)]
    [InlineData((short)2101)]
    [InlineData((short)32767)]
    public void Character_given_invalid_firstAppearance_is_not_valid(short? firstAppearance)
    {
        var character = new Character { FirstAppearance = firstAppearance };

        var result = _validator.TestValidate(character);

        result.ShouldHaveValidationErrorFor(character => character.FirstAppearance);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("  ")]
    [InlineData("https://images.com/riddler.png?reload=cdbc3e1b-0dfb-4a3a-853c-d26647c2f7be-ab9b99b0-08ac-4a95-93e6-74ea36e03beb.db0b4bb1-e19c-455c-9f6f-b218df723b3e-8cc3bd9f-fb54-4011-835a-b066e99eb094.ddca13a7-e3d8-4ebe-a9dc-417e7bf843fc-f607aaa9-e877-47a6-9f9f-8dbf2272b6d5")] // Too long
    public void Character_given_empty_imageUrl_is_not_valid(string imageUrl)
    {
        var character = _character with
        {
            ImageUrl = imageUrl,
        };

        var result = _validator.TestValidate(character);

        result.ShouldHaveValidationErrorFor(character => character.ImageUrl);
    }

    [Theory]
    [InlineData("cdbc3e1b-0dfb-4a3a-853c-d26647c2f7be")]
    [InlineData("http://images.com/riddler.png")] // Not https
    public void Character_given_invalid_imageUrl_is_not_valid(string imageUrl)
    {
        var character = _character with
        {
            ImageUrl = imageUrl,
        };

        var result = _validator.TestValidate(character);

        result.ShouldHaveValidationErrorFor(character => character.ImageUrl).WithErrorMessage("Image url must be either null or a valid url using https.");
    }

    [Fact]
    public void Character_given_invalid_gender_is_not_valid()
    {
        var character = _character with { Gender = (Gender)(-1) };

        var result = _validator.TestValidate(character);

        result.ShouldHaveValidationErrorFor(character => character.Gender);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("  ")]
    [InlineData("cdbc3e1b-0dfb-4a3a-853c-d26647c2f7be-529c0491-69ad-4970-90d7-8fa4cddb2556")]
    public void Character_given_invalid_power_is_not_valid(string power)
    {
        var character = _character with { Powers = new HashSet<string> { power } };

        var result = _validator.TestValidate(character);

        result.ShouldHaveValidationErrorFor(character => character.Powers);
    }
}
