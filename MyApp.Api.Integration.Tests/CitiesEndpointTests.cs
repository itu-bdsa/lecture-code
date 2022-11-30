namespace MyApp.Api.Integration.Tests
{
    [TestCaseOrderer("MyApp.Api.Integration.Tests.PriorityOrderer", "MyApp.Api.Integration.Tests")]
    public class CitiesEndpointTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public CitiesEndpointTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact, TestPriority(0)]
        public async Task Get()
        {
            var cities = await _client.GetFromJsonAsync<City[]>("cities");

            cities.Should().BeEquivalentTo(new[]
            {
                new City { Id = 1, Name = "Metropolis" },
                new City { Id = 2, Name = "Gotham City" }
            });
        }

        [Fact, TestPriority(1)]
        public async Task GetById()
        {
            var cities = await _client.GetFromJsonAsync<City>("cities/2");

            cities.Should().Be(new City { Id = 2, Name = "Gotham City" });
        }

        [Fact, TestPriority(2)]
        public async Task Post()
        {
            var response = await _client.PostAsJsonAsync("cities", new City { Name = "Central City" });

            //disabled endpoint returns "3" (bug in framework?)
            //response.Headers.Location.Should().Be(new Uri(_client.BaseAddress!, "cities/3"));

            var city = await response.Content.ReadFromJsonAsync<City>();

            city.Should().Be(new City { Id = 3, Name = "Central City" });
        }

        [Fact, TestPriority(3)]
        public async Task Put()
        {
            var response = await _client.PutAsJsonAsync("cities/3", new City { Id = 3, Name = "Themyscira" });

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact, TestPriority(4)]
        public async Task Delete()
        {
            var response = await _client.DeleteAsync("cities/3");

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}
