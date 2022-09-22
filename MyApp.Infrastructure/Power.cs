namespace MyApp.Infrastructure;

public class Power
{
    public int Id { get; set; }
    public string Name { get; set; }
    public virtual ICollection<Character> Characters { get; set; }

    public Power(string name)
    {
        Characters = new HashSet<Character>();
        Name = name;
    }
}
