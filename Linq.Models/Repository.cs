using static Linq.Models.Gender;

namespace Linq.Models;

public class Repository
{
    public ICollection<Superhero> Superheroes { get; }

    public ICollection<City> Cities { get; }

    public Repository()
    {
        Cities = new HashSet<City>
        {
            new City { Id = 1, Name = "Metropolis" },
            new City { Id = 2, Name = "Gotham City" },
            new City { Id = 3, Name = "Themyscira" },
            new City { Id = 4, Name = "New York City" },
            new City { Id = 5, Name = "Central City" }
        };

        Superheroes = new HashSet<Superhero>
        {
            new Superhero(1, "Clark", "Kent", "Superman", "Reporter", Male, 1938, 1),
            new Superhero(2, "Bruce", "Wayne", "Batman", "CEO of Wayne Enterprises", Male, 1939, 2),
            new Superhero(3, "Diana", "Prince", "Wonder Woman", "Amazon Princess", Female, 1941, 3),
            new Superhero(4, "Hal", "Jordan", "Green Lantern", "Test pilot", Male, 1940, 4),
            new Superhero(5, "Barry", "Allen", "The Flash", "Forensic scientist", Male, 1940, 5),
            new Superhero(6, "Selina", "Kyle", "Catwoman", "Thief", Female, 1940, 2)
        };
    }
}
