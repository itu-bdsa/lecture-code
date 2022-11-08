using Microsoft.AspNetCore.Mvc;
using MyApp.Core;

namespace MyApp.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CitiesController : ControllerBase
{
    private readonly ILogger<CitiesController> _logger;

    private readonly ICityRepository _repository;

    public CitiesController(ILogger<CitiesController> logger, ICityRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    [HttpGet]
    public async Task<IEnumerable<CityDto>> Get() => await _repository.ReadAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<CityDto>> Get(int id)
    {
        var city = await _repository.FindAsync(id);

        if (city is null)
        {
            return NotFound();
        }

        return city;
    }

    [HttpPost]
    public async Task<IActionResult> Post(CityCreateDto city)
    {
        var (status, created) = await _repository.CreateAsync(city);

        if (status == Status.Created)
        {
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        return Conflict(created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, CityDto city)
    {
        var result = await _repository.UpdateAsync(city);

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