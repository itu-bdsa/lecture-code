using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DataAccess;

internal class FuturamaContextFactory : IDesignTimeDbContextFactory<FuturamaContext>
{
    public FuturamaContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
        var connectionString = configuration.GetConnectionString("Futurama");

        var optionsBuilder = new DbContextOptionsBuilder<FuturamaContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new FuturamaContext(optionsBuilder.Options);
    }
}