namespace MyApp.Api.Tests;

public class CitiesControllerTests
{
    private readonly ICityRepository _repository;
    private readonly CitiesController _sut;

    public CitiesControllerTests()
    {
        var logger = Substitute.For<ILogger<CitiesController>>();
        _repository = Substitute.For<ICityRepository>();
        _sut = new CitiesController(logger, _repository);
    }

    [Fact]
    public async Task Get_NonExisting() => (await _sut.Get(42)).Result.Should().BeAssignableTo<NotFoundResult>();

    [Fact]
    public async Task Get_Existing()
    {
        var city = new CityDto(3, "Central City");

        _repository.FindAsync(3).Returns(city);

        var response = await _sut.Get(3);

        response.Value.Should().Be(city);
    }

    [Fact]
    public async Task Get_All()
    {
        var cities = new CityDto[] { new(1, "Gotham City"), new(2, "Metropolis") };

        _repository.ReadAsync().Returns(cities);

        var response = await _sut.Get();

        response.Should().BeEquivalentTo(cities);
    }

    [Fact]
    public async Task Post_New()
    {
        var create = new CityCreateDto("Metropolis");
        var created = new CityDto(2, "Metropolis");

        _repository.CreateAsync(create).Returns((Status.Created, created));

        var response = await _sut.Post(create);
        var result = response as CreatedAtActionResult;

        result!.ActionName.Should().Be(nameof(CitiesController.Get));
        result.RouteValues!.Single().Should().Be(new KeyValuePair<string, object>("id", 2));
        result.Value.Should().Be(created);
    }

    [Fact]
    public async Task Post_Existing()
    {
        var create = new CityCreateDto("Gotham City");
        var existing = new CityDto(1, "Gotham City");

        _repository.CreateAsync(create).Returns((Status.Conflict, existing));

        var response = await _sut.Post(create);
        var result = response as ConflictObjectResult;

        result!.Value.Should().Be(existing);
    }

    [Fact]
    public async Task Put_Existing()
    {
        var update = new CityDto(1, "Gotham City");

        _repository.UpdateAsync(update).Returns(Status.Updated);

        var response = await _sut.Put(1, update);

        response.Should().BeAssignableTo<NoContentResult>();
    }

    [Fact]
    public async Task Put_Existing_Conflicting_Name()
    {
        var update = new CityDto(1, "Metropolis");

        _repository.UpdateAsync(update).Returns(Status.Conflict);

        var response = await _sut.Put(1, update);

        response.Should().BeAssignableTo<ConflictResult>();
    }

    [Fact]
    public async Task Put_NonExisting()
    {
        var update = new CityDto(42, "Smallville");

        _repository.UpdateAsync(update).Returns(Status.NotFound);

        var response = await _sut.Put(42, update);

        response.Should().BeAssignableTo<NotFoundResult>();
    }

    [Fact]
    public async Task Delete_Existing()
    {
        _repository.DeleteAsync(1).Returns(Status.Deleted);

        var response = await _sut.Delete(1);

        response.Should().BeAssignableTo<NoContentResult>();
    }

    [Fact]
    public async Task Delete_NonExisting()
    {
        _repository.DeleteAsync(42).Returns(Status.NotFound);

        var response = await _sut.Delete(42);

        response.Should().BeAssignableTo<NotFoundResult>();
    }

    [Fact]
    public async Task Delete_Existing_Conflict()
    {
        _repository.DeleteAsync(2).Returns(Status.Conflict);

        var response = await _sut.Delete(2);

        response.Should().BeAssignableTo<ConflictResult>();
    }
}