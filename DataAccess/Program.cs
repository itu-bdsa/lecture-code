using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();
var connectionString = configuration.GetConnectionString("ConnectionString");

var optionsBuilder = new DbContextOptionsBuilder<FuturamaContext>();
optionsBuilder.UseSqlServer(connectionString);

var options = optionsBuilder.Options;

Console.Write("Enter query to search for character: ");

string? name;
do
{
    Console.Write("Enter query to search for character: ");
    name = Console.ReadLine();
}
while (string.IsNullOrWhiteSpace(name));

using var context = new FuturamaContext(options);

var characters = from c in context.Characters
                 where c.Name.Contains(name)
                 orderby c.Name
                 select new
                 {
                     c.Name,
                     c.Species,
                     c.Planet,
                     Actor = c.Actor == null ? string.Empty : c.Actor.Name
                 };

foreach (var character in characters)
{
    Console.WriteLine(character);
}

// var cmdText = @"SELECT c.Name, c.Species, a.Name as Actor
//                 FROM Characters AS c
//                 JOIN Actors AS a ON c.ActorId = a.Id
//                 WHERE c.Name LIKE '%' + @Name + '%'";

// using var connection = new SqlConnection(connectionString);
// using var command = new SqlCommand(cmdText, connection);
// command.Parameters.AddWithValue("@Name", name);
// connection.Open();

// using var reader = command.ExecuteReader();

// while (reader.Read())
// {
//     var character = new
//     {
//         Name = reader.GetString("Name"),
//         Species = reader.GetString("Species"),
//         Actor = reader.GetString("Actor")
//     };

//     Console.WriteLine(character);
// }