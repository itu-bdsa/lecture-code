namespace MyApp.Infrastructure;

public class City
{
    public int Id { get; set; }
    public string Name { get; set; }
    public virtual ICollection<Character> Characters { get; set; }

    public City(string name)
    {
        Characters = new HashSet<Character>();
        Name = name;
    }
}
