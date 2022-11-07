namespace MyApp.Integration.Tests.Setup;

public class IntegrationTestFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly TestcontainerDatabase _container;

    public IntegrationTestFactory()
    {
        _container = new TestcontainersBuilder<MsSqlTestcontainer>()
            .WithDatabase(new MsSqlTestcontainerConfiguration
            {
                Database = "Comics",
                Password = "hgbEjUipSFuED2AX1xoFqKyAOw8=",
            })
            .WithImage("mcr.microsoft.com/mssql/server:2019-latest")
            .WithCleanUp(true)
            .Build();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveDbContext<ComicsContext>();
            services.AddDbContext<ComicsContext>(options => { options.UseSqlServer(_container.ConnectionString); });
            services.MigrateDbContext<ComicsContext>();
            services.LoadTestData<ComicsContext>(TestDataGenerator.GenerateTestData);
        });
    }

    public async Task InitializeAsync() => await _container.StartAsync();

    public new async Task DisposeAsync() => await _container.DisposeAsync();
}
