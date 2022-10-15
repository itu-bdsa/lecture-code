var factory = new ComicsContextFactory();
using var context = factory.CreateDbContext(args);
await context.Database.MigrateAsync();

if (args.Length > 0 && args[0] == "seed")
{
    Console.WriteLine("Resetting seed data");

    await Seed.SeedAsync(context);

    var harleyQuinn = new CharacterCreateDto(
        AlterEgo: "Harley Quinn",
        GivenName: "Harleen",
        Surname: "Quinzel",
        FirstAppearance: 1992,
        Occupation: "Former psychiatrist",
        City: "Gotham City",
        Gender: Female,
        ImageUrl: "https://upload.wikimedia.org/wikipedia/en/a/ab/Harley_Quinn_Infobox.png",
        Powers: new HashSet<string> { "complete unpredictability", "superhuman agility", "skilled fighter", "intelligence", "emotional manipulation", "immunity to toxins" }
    );

    var repository = new CharacterRepository(context);
    await repository.CreateAsync(harleyQuinn);
}

var characters = await context.Characters.CountAsync();
var cities = await context.Cities.CountAsync();
var powers = await context.Powers.CountAsync();

Console.WriteLine($"Database contains: {characters} characters, {cities} cities, and {powers} powers");
