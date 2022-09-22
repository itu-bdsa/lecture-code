namespace DataAccess;

public class Actor
{
    public Actor()
    {
        Characters = new HashSet<Character>();
    }

    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public virtual ICollection<Character> Characters { get; set; }
}
