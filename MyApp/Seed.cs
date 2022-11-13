namespace MyApp;

public static class Seed
{
    public static async Task SeedAsync(ComicsContext context)
    {
        await context.Database.ExecuteSqlRawAsync("DELETE FROM dbo.Characters");
        await context.Database.ExecuteSqlRawAsync("DELETE FROM dbo.Cities");
        await context.Database.ExecuteSqlRawAsync("DELETE FROM dbo.Powers");
        await context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('dbo.Characters', RESEED, 0)");
        await context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('dbo.Cities', RESEED, 0)");
        await context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('dbo.Powers', RESEED, 0)");

        var metropolis = new CityEntity { Name = "Metropolis" };
        var gothamCity = new CityEntity { Name = "Gotham City" };
        var themyscira = new CityEntity { Name = "Themyscira" };

        context.Cities.AddRange(metropolis, gothamCity, themyscira);
        await context.SaveChangesAsync();

        var superStrength = new PowerEntity { Name = "super strength" };
        var flight = new PowerEntity { Name = "flight" };
        var invulnerability = new PowerEntity { Name = "invulnerability" };
        var superSpeed = new PowerEntity { Name = "super speed" };
        var heatVision = new PowerEntity { Name = "heat vision" };
        var freezeBreath = new PowerEntity { Name = "freeze breath" };
        var xRayVision = new PowerEntity { Name = "x-ray vision" };
        var superhumanHearing = new PowerEntity { Name = "superhuman hearing" };
        var healingFactor = new PowerEntity { Name = "healing factor" };
        var exceptionalMartialArtist = new PowerEntity { Name = "exceptional martial artist" };
        var combatStrategy = new PowerEntity { Name = "combat strategy" };
        var inexhaustibleWealth = new PowerEntity { Name = "inexhaustible wealth" };
        var brilliantDeductiveSkill = new PowerEntity { Name = "brilliant deductive skill" };
        var advancedTechnology = new PowerEntity { Name = "advanced technology" };
        var combatSkill = new PowerEntity { Name = "combat skill" };
        var superhumanAgility = new PowerEntity { Name = "superhuman weaponry" };
        var magicWeaponry = new PowerEntity { Name = "magic agility" };
        var gymnasticAbility = new PowerEntity { Name = "gymnastic ability" };

        context.Powers.AddRange(superStrength, flight, invulnerability, superSpeed, heatVision, freezeBreath, xRayVision, superhumanHearing, healingFactor, exceptionalMartialArtist, combatStrategy, inexhaustibleWealth, brilliantDeductiveSkill, advancedTechnology, combatSkill, superhumanAgility, magicWeaponry, gymnasticAbility);
        await context.SaveChangesAsync();

        context.Characters.AddRange(
            new CharacterEntity { GivenName = "Clark", Surname = "Kent", AlterEgo = "Superman", Occupation = "Reporter", City = metropolis, Gender = Male, FirstAppearance = 1938, ImageUrl = "https://upload.wikimedia.org/wikipedia/en/3/35/Supermanflying.png", Powers = new[] { superStrength, flight, invulnerability, superSpeed, heatVision, freezeBreath, xRayVision, superhumanHearing, healingFactor } },
            new CharacterEntity { GivenName = "Bruce", Surname = "Wayne", AlterEgo = "Batman", Occupation = "CEO of Wayne Enterprises", City = gothamCity, Gender = Male, FirstAppearance = 1939, ImageUrl = "https://upload.wikimedia.org/wikipedia/en/c/c7/Batman_Infobox.jpg", Powers = new[] { exceptionalMartialArtist, combatStrategy, inexhaustibleWealth, brilliantDeductiveSkill, advancedTechnology } },
            new CharacterEntity { GivenName = "Diana", Surname = "Prince", AlterEgo = "Wonder Woman", Occupation = "Amazon Princess", City = themyscira, Gender = Female, FirstAppearance = 1941, ImageUrl = "https://upload.wikimedia.org/wikipedia/en/9/93/Wonder_Woman.jpg", Powers = new[] { superStrength, invulnerability, flight, combatSkill, combatStrategy, superhumanAgility, healingFactor, magicWeaponry } },
            new CharacterEntity { GivenName = "Selina", Surname = "Kyle", AlterEgo = "Catwoman", Occupation = "Thief", City = gothamCity, Gender = Female, FirstAppearance = 1940, ImageUrl = "https://upload.wikimedia.org/wikipedia/en/e/e4/Catwoman_Infobox.jpg", Powers = new[] { exceptionalMartialArtist, gymnasticAbility, combatSkill } }
        );
        await context.SaveChangesAsync();
    }
}