namespace MyApp.Infrastructure;

public record City
{
    public int Id { get; set; }

    [StringLength(100)]
    public string Name { get; set; }

    public City(string name)
    {
        Name = name;
    }
}
