using Microsoft.AspNetCore.Mvc;
using MyApp.Core;

namespace MyApp.Server.Controllers;

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
    public async Task<IEnumerable<CharacterDto>> Get() => await _repository.ReadAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<CharacterDto>> Get(int id)
    {
        var character = await _repository.FindAsync(id);

        if (character is null)
        {
            return NotFound();
        }

        return character;
    }

    [HttpPost]
    public async Task<IActionResult> Post(CharacterCreateDto character)
    {
        var created = await _repository.CreateAsync(character);

        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, CharacterUpdateDto character)
    {
        var result = await _repository.UpdateAsync(character);

        return result == Status.Updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _repository.DeleteAsync(id);

        return result == Status.Deleted ? NoContent() : NotFound();
    }
}