namespace MyApp.Infrastructure;

public class CityEntity
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public virtual ICollection<CharacterEntity> Characters { get; set; }

    public CityEntity()
    {
        Characters = new HashSet<CharacterEntity>();
    }
}
