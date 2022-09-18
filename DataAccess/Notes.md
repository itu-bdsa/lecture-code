# Notes

## Run SQL Server container

```bash
MSSQL_SA_PASSWORD='<YourStrong@Passw0rd>'

docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=$MSSQL_SA_PASSWORD" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2019-latest

CONNECTION_STRING="Server=localhost;Database=Futurama;User Id=sa;Password=<YourStrong@Passw0rd>;Trusted_Connection=False;Encrypt=False"
```

## Enable User Secrets

```bash
CONNECTION_STRING="Server=localhost;Database=Futurama;User Id=sa;Password=<YourStrong@Passw0rd>;Trusted_Connection=False;Encrypt=False"

dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:Futurama" "$CONNECTION_STRING"
dotnet add package Microsoft.Extensions.Configuration.UserSecrets
```

## Get Connection String

```csharp
var configuration = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
var connectionString = configuration.GetConnectionString("Futurama");
```

## Raw SQL

```csharp
var cmdText = @"SELECT c.Name, c.Species, a.Name as Actor
                FROM Characters AS c
                JOIN Actors AS a ON c.ActorId = a.Id
                ORDER BY c.Name";

using var connection = new SqlConnection(connectionString);
using var command = new SqlCommand(cmdText, connection);

connection.Open();

using var reader = command.ExecuteReader();

while (reader.Read())
{
    var character = new {
        Name = reader.GetString("Name"),
        Species = reader.GetString("Species"),
        Actor = reader.GetString("Actor")
    };

    Console.WriteLine(character);
}
```

## Install the Entity Framework global tool

```bash
dotnet tool install --global dotnet-ef
dotnet ef migrations add Start --startup-project DataAccess --project DataAccess.Infrastructure
dotnet ef database update --startup-project DataAccess --project DataAccess.Infrastructure
```

## Reverse Engineer Database

```bash
CONNECTION_STRING="Server=localhost;Database=Futurama;User Id=sa;Password=<YourStrong@Passw0rd>;Trusted_Connection=False;Encrypt=False"

dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.SqlServer

dotnet ef dbcontext scaffold "$CONNECTION_STRING" Microsoft.EntityFrameworkCore.SqlServer --data-annotations
```

## Install the SQL client package

```bash
dotnet add package Microsoft.Data.SqlClient
```

## Entities project

```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
```

## Add migration

```bash
dotnet ef migrations add InitialMigration
```

## Update database

```bash
dotnet ef database update
```

## ComicsContext

```csharp
public ComicsContext(DbContextOptions<ComicsContext> options) : base(options) { }

protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder
        .Entity<Character>()
        .Property(e => e.Gender)
        .HasConversion(new EnumToStringConverter<Gender>());
}
```

## `IDesignTimeDbContextFactory<T>`

```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DataAccess;

internal class ComicsContextFactory : IDesignTimeDbContextFactory<ComicsContext>
{
    public ComicsContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
        var connectionString = configuration.GetConnectionString("Comics");

        var optionsBuilder = new DbContextOptionsBuilder<ComicsContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new ComicsContext(optionsBuilder.Options);
    }
}
```

## Seed

```csharp
public static void Seed(ComicsContext context)
{
    context.Database.ExecuteSqlRaw("DELETE dbo.PowerCharacter");
    context.Database.ExecuteSqlRaw("DELETE dbo.Characters");
    context.Database.ExecuteSqlRaw("DELETE dbo.Powers");
    context.Database.ExecuteSqlRaw("DELETE dbo.Cities");
    context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('dbo.Powers', RESEED, 0)");
    context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('dbo.Cities', RESEED, 0)");
    context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('dbo.Characters', RESEED, 0)");

    var metropolis = new City { Name = "Metropolis" };
    var gothamCity = new City { Name = "Gotham City" };
    var themyscira = new City { Name = "Themyscira" };

    var superStrength = new Power { Name = "super strength" };
    var flight = new Power { Name = "flight" };
    var invulnerability = new Power { Name = "invulnerability" };
    var superSpeed = new Power { Name = "super speed" };
    var heatVision = new Power { Name = "heat vision" };
    var freezeBreath = new Power { Name = "freeze breath" };
    var xRayVision = new Power { Name = "x-ray vision" };
    var superhumanHearing = new Power { Name = "superhuman hearing" };
    var healingFactor = new Power { Name = "healing factor" };
    var exceptionalMartialArtist = new Power { Name = "exceptional martial artist" };
    var combatStrategy = new Power { Name = "combat strategy" };
    var inexhaustibleWealth = new Power { Name = "inexhaustible wealth" };
    var brilliantDeductiveSkill = new Power { Name = "brilliant deductive skill" };
    var advancedTechnology = new Power { Name = "advanced technology" };
    var combatSkill = new Power { Name = "combat skill technology" };
    var superhumanAgility = new Power { Name = "superhuman weaponry" };
    var magicWeaponry = new Power { Name = "magic agility" };
    var gymnasticAbility = new Power { Name = "gymnastic ability" };

    context.Superheroes.AddRange(
        new Character { Name = "Clark Kent", AlterEgo = "Superman", Occupation = "Reporter", City = metropolis, Gender = Male, FirstAppearance = DateTime.Parse("1938-04-18"), Powers = new[] { superStrength, flight, invulnerability, superSpeed, heatVision, freezeBreath, xRayVision, superhumanHearing, healingFactor } },
        new Character { Name = "Bruce Wayne", AlterEgo = "Batman", Occupation = "CEO of Wayne Enterprises", City = gothamCity, Gender = Male, FirstAppearance = DateTime.Parse("1939-05-01"), Powers = new[] { exceptionalMartialArtist, combatStrategy, inexhaustibleWealth, brilliantDeductiveSkill, advancedTechnology } },
        new Character { Name = "Diana", AlterEgo = "Wonder Woman", Occupation = "Amazon Princess", City = themyscira, Gender = Female, FirstAppearance = DateTime.Parse("1941-10-21"), Powers = new[] { superStrength, invulnerability, flight, combatSkill, combatStrategy, superhumanAgility, healingFactor, magicWeaponry } },
        new Character { Name = "Selina Kyle", AlterEgo = "Catwoman", Occupation = "Thief", City = gothamCity, Gender = Female, FirstAppearance = DateTime.Parse("1940-04-01"), Powers = new[] { exceptionalMartialArtist, gymnasticAbility, combatSkill } }
    );

    context.SaveChanges();
}
```

## Harley Quinn

Name: Harleen Quinzel
Alter Ego: Harley Quinn
Occupation: Former psychiatrist
City: Gotham City
Powers: complete unpredictability, superhuman agility, skilled fighter, intelligence, emotional manipulation, immunity to toxins
FirstAppearance: September 11, 1992
