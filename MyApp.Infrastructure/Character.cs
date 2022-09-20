

namespace MyApp.Infrastructure;

public record Character
{
    public int Id { get; set; }

    public string? GivenName { get; set; }

    public string? Surname { get; set; }

    public string? AlterEgo { get; set; }
    public string? Occupation { get; set; }
    public Gender Gender { get; set; }
    public int? FirstAppearance { get; set; }
    public int? CityId { get; set; }

    public City? City { get; set; }
}
