using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyApp.Infrastructure;

namespace MyApp.Api.Integration.Tests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ComicsContext>));

                if (descriptor is not null)
                {
                    services.Remove(descriptor);
                }

                var connectionString = $"Server=localhost;Database={Guid.NewGuid()};User Id=sa;Password=<YourStrong@Passw0rd>;Trusted_Connection=False;Encrypt=False";

                services.AddDbContext<ComicsContext>(options =>
                {
                    options.UseSqlServer(connectionString);
                });
            });

            builder.UseEnvironment("Development");
        }

        public async Task InitializeAsync()
        {
            using var scope = Services.CreateAsyncScope();
            using var context = scope.ServiceProvider.GetRequiredService<ComicsContext>();
            await context.Database.MigrateAsync();

            var metropolis = new CityEntity { Name = "Metropolis" };
            var gothamCity = new CityEntity { Name = "Gotham City" };

            context.Cities.AddRange(metropolis, gothamCity);
            await context.SaveChangesAsync();

            var exceptionalMartialArtist = new PowerEntity { Name = "exceptional martial artist" };
            var combatSkill = new PowerEntity { Name = "combat skill" };
            var gymnasticAbility = new PowerEntity { Name = "gymnastic ability" };

            context.Powers.AddRange(exceptionalMartialArtist, combatSkill, gymnasticAbility);
            await context.SaveChangesAsync();

            context.Characters.AddRange(
                new CharacterEntity { GivenName = "Clark", Surname = "Kent", AlterEgo = "Superman"},
                new CharacterEntity { GivenName = "Bruce", Surname = "Wayne", AlterEgo = "Batman" }
            );
            await context.SaveChangesAsync();
        }

        async Task IAsyncLifetime.DisposeAsync()
        {
            using var scope = Services.CreateAsyncScope();
            using var context = scope.ServiceProvider.GetRequiredService<ComicsContext>();

            await context.Database.EnsureDeletedAsync();
        }
    }
}