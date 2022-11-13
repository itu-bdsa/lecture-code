using FluentValidation;

namespace MyApp.Core;

public record BasicCharacter
{
    public int Id { get; init; }
    public string? AlterEgo { get; init; }
    public string? GivenName { get; init; }
    public string? Surname { get; init; }
}

public sealed record Character : BasicCharacter
{
    public short? FirstAppearance { get; init; }
    public string? Occupation { get; init; }
    public string? City { get; init; }
    public Gender? Gender { get; init; }
    public string? ImageUrl { get; init; }
    public ISet<string> Powers { get; init; } = new HashSet<string>();
}

public sealed class CharacterValidator : AbstractValidator<Character>
{
    public CharacterValidator()
    {
        RuleFor(x => x).Must(Have_at_least_one_of_AlterEgo_GivenName_Surname).WithMessage("At least one of alter ego, given name, or surname must be supplied.");
        RuleFor(x => x.AlterEgo).NotEmpty().Unless(x => x.AlterEgo == null).MaximumLength(50);
        RuleFor(x => x.GivenName).NotEmpty().Unless(x => x.GivenName == null).MaximumLength(50);
        RuleFor(x => x.Surname).NotEmpty().Unless(x => x.Surname == null).MaximumLength(50);
        RuleFor(x => x.FirstAppearance).InclusiveBetween((short)1900, (short)2100);
        RuleFor(x => x.Occupation).NotEmpty().Unless(x => x.Occupation == null).MaximumLength(50);
        RuleFor(x => x.City).NotEmpty().Unless(x => x.City == null).MaximumLength(50);
        RuleFor(x => x.Gender).IsInEnum();
        RuleFor(x => x.ImageUrl).NotEmpty().Unless(x => x.ImageUrl == null).MaximumLength(250).Must(Be_a_valid_url_or_null).WithMessage("Image url must be either null or a valid url using https.");
        RuleForEach(x => x.Powers).NotEmpty().MaximumLength(50);
    }

    private bool Have_at_least_one_of_AlterEgo_GivenName_Surname(BasicCharacter character) =>
        !string.IsNullOrWhiteSpace(character.AlterEgo) ||
        !string.IsNullOrWhiteSpace(character.GivenName) ||
        !string.IsNullOrWhiteSpace(character.Surname);

    private bool Be_a_valid_url_or_null(string? url)
    {
        if (url == null)
        {
            return true;
        }

        var options = new UriCreationOptions();

        if (Uri.TryCreate(url, options, out var uri))
        {
            return uri.Scheme == "https";
        }

        return false;
    }
}
