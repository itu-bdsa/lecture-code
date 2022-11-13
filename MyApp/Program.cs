var factory = new ComicsContextFactory();
using var context = factory.CreateDbContext(args);
await context.Database.MigrateAsync();

if (args.Length > 0 && args[0] == "seed")
{
    Console.WriteLine("Resetting seed data");

    await Seed.SeedAsync(context);

    // var riddler = new Character(
    //     AlterEgo: "Riddler",
    //     GivenName: "Edward",
    //     Surname: "Nygma",
    //     FirstAppearance: 1948,
    //     Occupation: "Professional criminal",
    //     City: "Gotham City",
    //     Gender: Male,
    //     ImageUrl: "https://upload.wikimedia.org/wikipedia/en/6/68/Riddler.png",
    //     Powers: new HashSet<string> { "genius-level intellect" }
    // );

    //var repository = new CharacterRepository(context);
    //await repository.CreateAsync(riddler);
}

var characters = await context.Characters.CountAsync();
var cities = await context.Cities.CountAsync();
var powers = await context.Powers.CountAsync();

Console.WriteLine($"Database contains: {characters} characters, {cities} cities, and {powers} powers");
