namespace MyApp.Api.Tests;

public class CharactersControllerTests
{
    private readonly ICharacterRepository _repository;
    private readonly CharactersController _sut;


    public CharactersControllerTests()
    {
        var logger = Substitute.For<ILogger<CharactersController>>();
        _repository = Substitute.For<ICharacterRepository>();
        _sut = new CharactersController(logger, _repository);
    }

    [Fact]
    public async Task Delete_given_not_found_returns_not_found()
    {
        _repository.DeleteAsync(42).Returns(Status.NotFound);

        var result = await _sut.Delete(42);

        result.Should().BeAssignableTo<NotFoundResult>();
    }

    [Fact]
    public async Task Delete_given_deleted_returns_NoContent()
    {
        _repository.DeleteAsync(42).Returns(Status.Deleted);

        var result = await _sut.Delete(42);

        result.Should().BeAssignableTo<NoContentResult>();
    }
}