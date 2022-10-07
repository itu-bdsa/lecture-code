namespace MyApp.Infrastructure;

public sealed class PowerRepository : IPowerRepository
{
    private readonly ComicsContext _context;

    public PowerRepository(ComicsContext context)
    {
        _context = context;
    }

    public async Task<(Status, PowerDto)> Create(PowerCreateDto power)
    {
        var entity = await _context.Powers.FirstOrDefaultAsync(c => c.Name == power.Name);
        Status status;

        if (entity is null)
        {
            entity = new Power(power.Name);

            _context.Powers.Add(entity);
            await _context.SaveChangesAsync();

            status = Created;
        }
        else
        {
            status = Conflict;
        }

        var created = new PowerDto(entity.Id, entity.Name);

        return (status, created);
    }

    public async Task<PowerDto?> Find(int powerId)
    {
        var cities = from c in _context.Powers
                     where c.Id == powerId
                     select new PowerDto(c.Id, c.Name);

        return await cities.FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyCollection<PowerDto>> Read()
    {
        var cities = from c in _context.Powers
                     orderby c.Name
                     select new PowerDto(c.Id, c.Name);

        return await cities.ToArrayAsync();
    }

    public async Task<Status> Update(PowerDto power)
    {
        var entity = await _context.Powers.FindAsync(power.Id);
        Status status;

        if (entity is null)
        {
            status = NotFound;
        }
        else if (await _context.Powers.FirstOrDefaultAsync(c => c.Id != power.Id && c.Name == power.Name) != null)
        {
            status = Conflict;
        }
        else
        {
            entity.Name = power.Name;
            await _context.SaveChangesAsync();
            status = Updated;
        }

        return status;
    }

    public async Task<Status> Delete(int powerId)
    {
        var power = await _context.Powers.Include(c => c.Characters).FirstOrDefaultAsync(c => c.Id == powerId);
        Status status;

        if (power is null)
        {
            status = NotFound;
        }
        else if (power.Characters.Any())
        {
            status = Conflict;
        }
        else
        {
            _context.Powers.Remove(power);
            await _context.SaveChangesAsync();

            status = Deleted;
        }

        return status;
    }
}