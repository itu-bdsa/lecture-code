using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
var connectionString = configuration.GetConnectionString("Futurama");

string? searchText;
do
{
    Console.Write("Enter search text: ");
    searchText = Console.ReadLine();
}
while (string.IsNullOrWhiteSpace(searchText));

Search(searchText);

void Search(string searchText)
{
    var cmdText = $"SELECT c.Name, c.Species, a.Name as Actor FROM Characters AS c JOIN Actors AS a ON c.ActorId = a.Id WHERE c.Name LIKE '%'+@SearchText+'%' ORDER BY c.Name";

    using var connection = new SqlConnection(connectionString);
    using var command = new SqlCommand(cmdText, connection);

    command.Parameters.AddWithValue("@SearchText", searchText);

    connection.Open();

    using var reader = command.ExecuteReader();

    while (reader.Read())
    {
        var character = new
        {
            Name = reader.GetString("Name"),
            Species = reader.GetString("Species"),
            Actor = reader.GetString("Actor")
        };

        Console.WriteLine(character);
    }
}