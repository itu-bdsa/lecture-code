using Microsoft.AspNetCore.Mvc;
using MyApp.Core;

namespace MyApp.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PowersController : ControllerBase
{
    private readonly ILogger<PowersController> _logger;

    private readonly IPowerRepository _repository;

    public PowersController(ILogger<PowersController> logger, IPowerRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    [HttpGet]
    public async Task<IEnumerable<PowerDto>> Get() => await _repository.ReadAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<PowerDto>> Get(int id)
    {
        var power = await _repository.FindAsync(id);

        if (power is null)
        {
            return NotFound();
        }

        return power;
    }

    [HttpPost]
    public async Task<IActionResult> Post(PowerCreateDto power)
    {
        var (status, created) = await _repository.CreateAsync(power);

        if (status == Status.Created)
        {
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        return Conflict(created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, PowerDto power)
    {
        var result = await _repository.UpdateAsync(power);

        return result switch
        {
            Status.NotFound => NotFound(),
            Status.Conflict => Conflict(),
            _ => NoContent()
        };
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _repository.DeleteAsync(id);

        return result switch
        {
            Status.NotFound => NotFound(),
            Status.Conflict => Conflict(),
            _ => NoContent()
        };
    }
}