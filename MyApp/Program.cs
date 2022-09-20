var configuration = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
var connectionString = configuration.GetConnectionString("Comics");

Console.WriteLine("Hello, World!");
