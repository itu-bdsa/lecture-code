namespace MyApp.Infrastructure;

public class Character
{
    public int Id { get; set; }
    public string? AlterEgo { get; set; }
    public string? GivenName { get; set; }
    public string? Surname { get; set; }
    public short? FirstAppearance { get; set; }
    public string? Occupation { get; set; }
    public int? CityId { get; set; }
    public virtual City? City { get; set; }
    public Gender Gender { get; set; }
    public string? ImageUrl { get; set; }
    public virtual ICollection<Power> Powers { get; set; }

    public Character()
    {
        Powers = new HashSet<Power>();
    }
}
