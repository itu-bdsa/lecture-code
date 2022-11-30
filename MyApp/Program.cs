var factory = new ComicsContextFactory();
using var context = factory.CreateDbContext(args);
await context.Database.EnsureDeletedAsync();
await context.Database.MigrateAsync();

if (args.Length > 0 && args[0] == "seed")
{
    var repository = new CharacterRepository(context, new CharacterValidator());

    var superman = new Character
    {
        GivenName = "Clark",
        Surname = "Kent",
        AlterEgo = "Superman",
        Occupation = "Reporter",
        City = "Metropolis",
        Gender = Male,
        FirstAppearance = 1938,
        ImageUrl = "https://upload.wikimedia.org/wikipedia/en/3/35/Supermanflying.png",
        Powers = new HashSet<string>
        {
            "super strength",
            "flight",
            "invulnerability",
            "super speed",
            "heat vision",
            "freeze breath",
            "x-ray vision",
            "superhuman hearing",
            "healing factor"
        }
    };

    await repository.CreateAsync(superman);

    var batman = new Character
    {
        GivenName = "Bruce",
        Surname = "Wayne",
        AlterEgo = "Batman",
        Occupation = "CEO of Wayne Enterprises",
        City = "Gotham City",
        Gender = Male,
        FirstAppearance = 1939,
        ImageUrl = "https://upload.wikimedia.org/wikipedia/en/c/c7/Batman_Infobox.jpg",
        Powers = new HashSet<string>
        {
            "exceptional martial artist",
            "combat strategy",
            "inexhaustible wealth",
            "brilliant deductive skill",
            "advanced technology"
        }
    };

    await repository.CreateAsync(batman);

    var wonderWoman = new Character
    {
        GivenName = "Diana",
        Surname = "Prince",
        AlterEgo = "Wonder Woman",
        Occupation = "Amazon Princess",
        City = "Themyscira",
        Gender = Female,
        FirstAppearance = 1941,
        ImageUrl = "https://upload.wikimedia.org/wikipedia/en/9/93/Wonder_Woman.jpg",
        Powers = new HashSet<string>
        {
            "super strength",
            "invulnerability",
            "flight",
            "combat skill",
            "combat strategy",
            "superhuman agility",
            "healing factor",
            "magic weaponry"
        }
    };

    await repository.CreateAsync(wonderWoman);

    var catwoman = new Character
    {
        GivenName = "Selina",
        Surname = "Kyle",
        AlterEgo = "Catwoman",
        Occupation = "Thief",
        City = "Gotham City",
        Gender = Female,
        FirstAppearance = 1940,
        ImageUrl = "https://upload.wikimedia.org/wikipedia/en/e/e4/Catwoman_Infobox.jpg",
        Powers = new HashSet<string>
        {
            "exceptional martial artist",
            "gymnastic ability",
            "combat skill"
        }
    };

    await repository.CreateAsync(catwoman);

    var riddler = new Character
    {
        AlterEgo = "Riddler",
        GivenName = "Edward",
        Surname = "Nygma",
        FirstAppearance = 1948,
        Occupation = "Professional criminal",
        City = "Gotham City",
        Gender = Male,
        ImageUrl = "https://upload.wikimedia.org/wikipedia/en/6/68/Riddler.png",
        Powers = new HashSet<string> { "genius-level intellect" }
    };

    await repository.CreateAsync(riddler);
}

var characters = await context.Characters.CountAsync();
var cities = await context.Cities.CountAsync();
var powers = await context.Powers.CountAsync();

Console.WriteLine($"Database contains = {characters} characters, {cities} cities, and {powers} powers");
