namespace MyApp.Infrastructure;

public class PowerEntity
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public virtual ICollection<CharacterEntity> Characters { get; set; }

    public PowerEntity()
    {
        Characters = new HashSet<CharacterEntity>();
    }
}
