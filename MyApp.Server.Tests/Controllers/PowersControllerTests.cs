namespace MyApp.Server.Tests;

public class PowersControllerTests
{
    private readonly Mock<IPowerRepository> _repository;
    private readonly PowersController _sut;

    public PowersControllerTests()
    {
        var logger = new Mock<ILogger<PowersController>>();
        _repository = new Mock<IPowerRepository>();
        _sut = new PowersController(logger.Object, _repository.Object);
    }

    [Fact]
    public async Task Get_NonExisting() => (await _sut.Get(42)).Result.Should().BeAssignableTo<NotFoundResult>();

    [Fact]
    public async Task Get_Existing()
    {
        var power = new PowerDto(3, "invulnerability");
        _repository.Setup(m => m.FindAsync(3)).ReturnsAsync(power);

        var response = await _sut.Get(3);

        response.Value.Should().Be(power);
    }

    [Fact]
    public async Task Get_All()
    {
        var cities = new PowerDto[] { new(1, "super strength"), new(2, "flight") };

        _repository.Setup(m => m.ReadAsync()).ReturnsAsync(cities);

        var response = await _sut.Get();

        response.Should().BeEquivalentTo(cities);
    }

    [Fact]
    public async Task Post_New()
    {
        var create = new PowerCreateDto("super speed");
        var created = new PowerDto(4, "super speed");

        _repository.Setup(m => m.CreateAsync(create)).ReturnsAsync((Status.Created, created));

        var response = await _sut.Post(create);
        var result = response as CreatedAtActionResult;

        result!.ActionName.Should().Be(nameof(PowersController.Get));
        result.RouteValues!.Single().Should().Be(new KeyValuePair<string, object>("id", 4));
        result.Value.Should().Be(created);
    }

    [Fact]
    public async Task Post_Existing()
    {
        var create = new PowerCreateDto("super strength");
        var existing = new PowerDto(1, "super strength");

        _repository.Setup(m => m.CreateAsync(create)).ReturnsAsync((Status.Conflict, existing));

        var response = await _sut.Post(create);
        var result = response as ConflictObjectResult;

        result!.Value.Should().Be(existing);
    }

    [Fact]
    public async Task Put_Existing()
    {
        var update = new PowerDto(1, "super strength");

        _repository.Setup(m => m.UpdateAsync(update)).ReturnsAsync(Status.Updated);

        var response = await _sut.Put(1, update);

        response.Should().BeAssignableTo<NoContentResult>();
    }

    [Fact]
    public async Task Put_Existing_Conflicting_Name()
    {
        var update = new PowerDto(1, "flight");

        _repository.Setup(m => m.UpdateAsync(update)).ReturnsAsync(Status.Conflict);

        var response = await _sut.Put(1, update);

        response.Should().BeAssignableTo<ConflictResult>();
    }

    [Fact]
    public async Task Put_NonExisting()
    {
        var update = new PowerDto(42, "superhuman weaponry");

        _repository.Setup(m => m.UpdateAsync(update)).ReturnsAsync(Status.NotFound);

        var response = await _sut.Put(42, update);

        response.Should().BeAssignableTo<NotFoundResult>();
    }

    [Fact]
    public async Task Delete_Existing()
    {
        _repository.Setup(m => m.DeleteAsync(1)).ReturnsAsync(Status.Deleted);

        var response = await _sut.Delete(1);

        response.Should().BeAssignableTo<NoContentResult>();
    }

    [Fact]
    public async Task Delete_NonExisting()
    {
        _repository.Setup(m => m.DeleteAsync(42)).ReturnsAsync(Status.NotFound);

        var response = await _sut.Delete(42);

        response.Should().BeAssignableTo<NotFoundResult>();
    }

    [Fact]
    public async Task Delete_Existing_Conflict()
    {
        _repository.Setup(m => m.DeleteAsync(2)).ReturnsAsync(Status.Conflict);

        var response = await _sut.Delete(2);

        response.Should().BeAssignableTo<ConflictResult>();
    }
}