// Inspired by: https://stackoverflow.com/questions/70982483/asp-net-core-integration-tests-with-dotnet-testcontainers

namespace MyApp.Integration.Tests.Setup;

public static class ServiceCollectionExtensions
{
    public static void RemoveDbContext<T>(this IServiceCollection services) where T : DbContext
    {
        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<T>));
        if (descriptor != null) services.Remove(descriptor);
    }

    public static void MigrateDbContext<T>(this IServiceCollection services) where T : DbContext
    {
        var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<T>();
        context.Database.EnsureCreated();
    }

    public static void LoadTestData<T>(this IServiceCollection services, Action<T> action) where T : DbContext
    {
        var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var context = scope.ServiceProvider.GetRequiredService<T>();

        action(context);
    }
}
