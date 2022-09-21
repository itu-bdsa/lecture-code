namespace MyApp.Core;

public record CharacterDto(int Id, string? AlterEgo, string? GivenName, string? Surname);

public record CharacterDetailsDto(int Id, string? AlterEgo, string? GivenName, string? Surname, string? City, Gender Gender, int? FirstAppearance, string? Occupation, string? ImageUrl, ISet<string> Powers) : CharacterDto(Id, AlterEgo, GivenName, Surname);

public record CharacterCreateDto(
    [StringLength(50)] string? AlterEgo,
    [StringLength(50)] string? GivenName,
    [StringLength(50)] string? Surname,
    [Range(1900, 2100)] short? FirstAppearance,
    [StringLength(50)] string? Occupation,
    [StringLength(50)] string? City,
    Gender Gender,
    [StringLength(250), Url] string? ImageUrl,
    [CustomValidation(typeof(CharacterValidation), nameof(CharacterValidation.ValidatePowers))] ISet<string> Powers
);

public record CharacterUpdateDto(
    int Id,
    [StringLength(50)] string? AlterEgo,
    [StringLength(50)] string? GivenName,
    [StringLength(50)] string? Surname,
    [Range(1900, 2100)] short? FirstAppearance,
    [StringLength(50)] string? Occupation,
    [StringLength(50)] string? City,
    Gender Gender,
    [StringLength(250), Url] string? ImageUrl,
    [CustomValidation(typeof(CharacterValidation), nameof(CharacterValidation.ValidatePowers))] ISet<string> Powers
) : CharacterCreateDto(AlterEgo, GivenName, Surname, FirstAppearance, Occupation, City, Gender, ImageUrl, Powers);

