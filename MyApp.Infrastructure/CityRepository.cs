namespace MyApp.Infrastructure;

public sealed class CityRepository : ICityRepository
{
    private readonly ComicsContext _context;
    private readonly CityValidator _validator;

    public CityRepository(ComicsContext context, CityValidator validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<Results<Created<City>, ValidationProblem, Conflict<City>>> CreateAsync(City city)
    {
        var validation = _validator.Validate(city);

        if (!validation.IsValid)
        {
            return TypedResults.ValidationProblem(validation.ToDictionary());
        }

        var entity = await _context.Cities.FirstOrDefaultAsync(c => c.Name == city.Name);

        if (entity is null)
        {
            entity = new CityEntity { Name = city.Name };
            _context.Cities.Add(entity);
            await _context.SaveChangesAsync();

            return TypedResults.Created($"{entity.Id}", city with { Id = entity.Id });
        }
        else
        {
            var existing = new City { Id = entity.Id, Name = entity.Name };

            return TypedResults.Conflict(existing);
        }
    }

    public async Task<Results<Ok<City>, NotFound<int>>> FindAsync(int id)
    {
        var cities = from c in _context.Cities
                     where c.Id == id
                     select new City { Id = c.Id, Name = c.Name };

        var city = await cities.FirstOrDefaultAsync();

        return city is null ? TypedResults.NotFound(id) : TypedResults.Ok(city);
    }

    public async Task<IReadOnlyCollection<City>> ReadAsync()
    {
        var cities = from c in _context.Cities
                     orderby c.Name
                     select new City { Id = c.Id, Name = c.Name };

        return await cities.ToArrayAsync();
    }

    public async Task<Results<NoContent, ValidationProblem, NotFound<int>, Conflict<City>>> UpdateAsync(int id, City city)
    {
        var validation = _validator.Validate(city);

        if (!validation.IsValid)
        {
            return TypedResults.ValidationProblem(validation.ToDictionary());
        }

        var entity = await _context.Cities.FindAsync(id);

        if (entity is null)
        {
            return TypedResults.NotFound(id);
        }

        var existing = await _context.Cities.FirstOrDefaultAsync(c => c.Id != city.Id && c.Name == city.Name);

        if (existing != null)
        {
            return TypedResults.Conflict(new City { Id = existing.Id, Name = existing.Name });
        }
        else
        {
            entity.Name = city.Name;
            await _context.SaveChangesAsync();

            return TypedResults.NoContent();
        }
    }

    public async Task<Results<NoContent, NotFound<int>, Conflict<City>>> DeleteAsync(int id)
    {
        var city = await _context.Cities.Include(c => c.Characters).FirstOrDefaultAsync(c => c.Id == id);

        if (city is null)
        {
            return TypedResults.NotFound(id);
        }
        else if (city.Characters.Any())
        {
            return TypedResults.Conflict(new City { Id = city.Id, Name = city.Name });
        }
        else
        {
            _context.Cities.Remove(city);
            await _context.SaveChangesAsync();

            return TypedResults.NoContent();
        }
    }
}