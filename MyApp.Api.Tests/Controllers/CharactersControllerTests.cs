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
}