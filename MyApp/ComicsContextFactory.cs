using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MyApp;

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