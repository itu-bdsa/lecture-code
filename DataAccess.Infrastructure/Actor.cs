namespace DataAccess;

public class Actor
{
    public int Id { get; set; }

    [StringLength(50)]
    public string Name { get; set; }

    public ICollection<Character> Characters { get; set; } = new HashSet<Character>();

    public Actor(string name)
    {
        Name = name;
    }
}
