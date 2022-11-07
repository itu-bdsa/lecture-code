namespace MyApp.Integration.Tests.Setup;

public static class TestDataGenerator
{
    public static void GenerateTestData(ComicsContext context)
    {
        var metropolis = new City("Metropolis");
        var gothamCity = new City("Gotham City");
        var themyscira = new City("Themyscira");

        var superStrength = new Power("super strength");
        var flight = new Power("flight");
        var invulnerability = new Power("invulnerability");
        var superSpeed = new Power("super speed");
        var heatVision = new Power("heat vision");
        var freezeBreath = new Power("freeze breath");
        var xRayVision = new Power("x-ray vision");
        var superhumanHearing = new Power("superhuman hearing");
        var healingFactor = new Power("healing factor");
        var exceptionalMartialArtist = new Power("exceptional martial artist");
        var combatStrategy = new Power("combat strategy");
        var inexhaustibleWealth = new Power("inexhaustible wealth");
        var brilliantDeductiveSkill = new Power("brilliant deductive skill");
        var advancedTechnology = new Power("advanced technology");
        var combatSkill = new Power("combat skill");
        var superhumanAgility = new Power("superhuman weaponry");
        var magicWeaponry = new Power("magic agility");
        var gymnasticAbility = new Power("gymnastic ability");

        context.Characters.AddRange(
            new Character { GivenName = "Clark", Surname = "Kent", AlterEgo = "Superman", Occupation = "Reporter", City = metropolis, Gender = Male, FirstAppearance = 1938, ImageUrl = "https://upload.wikimedia.org/wikipedia/en/3/35/Supermanflying.png", Powers = new[] { superStrength, flight, invulnerability, superSpeed, heatVision, freezeBreath, xRayVision, superhumanHearing, healingFactor } },
            new Character { GivenName = "Bruce", Surname = "Wayne", AlterEgo = "Batman", Occupation = "CEO of Wayne Enterprises", City = gothamCity, Gender = Male, FirstAppearance = 1939, ImageUrl = "https://upload.wikimedia.org/wikipedia/en/c/c7/Batman_Infobox.jpg", Powers = new[] { exceptionalMartialArtist, combatStrategy, inexhaustibleWealth, brilliantDeductiveSkill, advancedTechnology } },
            new Character { GivenName = "Diana", Surname = "Prince", AlterEgo = "Wonder Woman", Occupation = "Amazon Princess", City = themyscira, Gender = Female, FirstAppearance = 1941, ImageUrl = "https://upload.wikimedia.org/wikipedia/en/9/93/Wonder_Woman.jpg", Powers = new[] { superStrength, invulnerability, flight, combatSkill, combatStrategy, superhumanAgility, healingFactor, magicWeaponry } },
            new Character { GivenName = "Selina", Surname = "Kyle", AlterEgo = "Catwoman", Occupation = "Thief", City = gothamCity, Gender = Female, FirstAppearance = 1940, ImageUrl = "https://upload.wikimedia.org/wikipedia/en/e/e4/Catwoman_Infobox.jpg", Powers = new[] { exceptionalMartialArtist, gymnasticAbility, combatSkill } }
        );

        context.SaveChanges();
    }
}