namespace MyApp.Infrastructure;

public sealed class PowerRepository : IPowerRepository
{
    private readonly ComicsContext _context;
    private readonly PowerValidator _validator;

    public PowerRepository(ComicsContext context, PowerValidator validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<Results<Created<Power>, ValidationProblem, Conflict<Power>>> CreateAsync(Power power)
    {
        var validation = _validator.Validate(power);

        if (!validation.IsValid)
        {
            return TypedResults.ValidationProblem(validation.ToDictionary());
        }

        var entity = await _context.Powers.FirstOrDefaultAsync(c => c.Name == power.Name);

        if (entity is null)
        {
            entity = new PowerEntity { Name = power.Name };
            _context.Powers.Add(entity);
            await _context.SaveChangesAsync();

            return TypedResults.Created($"{entity.Id}", power with { Id = entity.Id });
        }
        else
        {
            var existing = new Power { Id = entity.Id, Name = entity.Name };

            return TypedResults.Conflict(existing);
        }
    }

    public async Task<Results<Ok<Power>, NotFound<int>>> FindAsync(int id)
    {
        var powers = from c in _context.Powers
                     where c.Id == id
                     select new Power { Id = c.Id, Name = c.Name };

        var power = await powers.FirstOrDefaultAsync();

        return power is null ? TypedResults.NotFound(id) : TypedResults.Ok(power);
    }

    public async Task<IReadOnlyCollection<Power>> ReadAsync()
    {
        var powers = from c in _context.Powers
                     orderby c.Name
                     select new Power { Id = c.Id, Name = c.Name };

        return await powers.ToArrayAsync();
    }

    public async Task<Results<NoContent, ValidationProblem, NotFound<int>, Conflict<Power>>> UpdateAsync(int id, Power power)
    {
        var validation = _validator.Validate(power);

        if (!validation.IsValid)
        {
            return TypedResults.ValidationProblem(validation.ToDictionary());
        }

        var entity = await _context.Powers.FindAsync(id);

        if (entity is null)
        {
            return TypedResults.NotFound(id);
        }

        var existing = await _context.Powers.FirstOrDefaultAsync(c => c.Id != power.Id && c.Name == power.Name);

        if (existing != null)
        {
            return TypedResults.Conflict(new Power { Id = existing.Id, Name = existing.Name });
        }
        else
        {
            entity.Name = power.Name;
            await _context.SaveChangesAsync();

            return TypedResults.NoContent();
        }
    }

    public async Task<Results<NoContent, NotFound<int>, Conflict<Power>>> DeleteAsync(int id)
    {
        var power = await _context.Powers.Include(c => c.Characters).FirstOrDefaultAsync(c => c.Id == id);

        if (power is null)
        {
            return TypedResults.NotFound(id);
        }
        else if (power.Characters.Any())
        {
            return TypedResults.Conflict(new Power { Id = power.Id, Name = power.Name });
        }
        else
        {
            _context.Powers.Remove(power);
            await _context.SaveChangesAsync();

            return TypedResults.NoContent();
        }
    }
}