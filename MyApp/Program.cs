using MyApp.Infrastructure;

var factory = new ComicsContextFactory();
using var context = factory.CreateDbContext(args);

// var character = new Character
// {
//     GivenName = "Harleen",
//     Surname = "Quinzel",
//     AlterEgo = "Harley Quinn"
// };

// context.Characters.Add(character);
// context.SaveChanges();

var harley = context.Characters.Find(1);

Console.WriteLine(harley);