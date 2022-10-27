using Microsoft.AspNetCore.Mvc;
using MyApp.Core;

namespace MyApp.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CharactersController : ControllerBase
{
    private readonly ILogger<CharactersController> _logger;

    private readonly ICharacterRepository _repository;

    public CharactersController(ILogger<CharactersController> logger, ICharacterRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    [HttpGet]
    public async Task<IEnumerable<CharacterDto>> Get() => throw new NotImplementedException();

    [HttpGet("{id}")]
    public async Task<ActionResult<CharacterDto>> Get(int id) => throw new NotImplementedException();

    [HttpPost]
    public async Task<IActionResult> Post(CharacterCreateDto character) => throw new NotImplementedException();

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, CharacterUpdateDto character) => throw new NotImplementedException();

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) => throw new NotImplementedException();
}