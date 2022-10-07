namespace MyApp.Infrastructure;

public sealed class CityRepository : ICityRepository
{
    private readonly ComicsContext _context;

    public CityRepository(ComicsContext context)
    {
        _context = context;
    }

    public async Task<(Status, CityDto)> Create(CityCreateDto city)
    {
        var entity = await _context.Cities.FirstOrDefaultAsync(c => c.Name == city.Name);
        Status status;

        if (entity is null)
        {
            entity = new City(city.Name);

            _context.Cities.Add(entity);
            await _context.SaveChangesAsync();

            status = Created;
        }
        else
        {
            status = Conflict;
        }

        var created = new CityDto(entity.Id, entity.Name);

        return (status, created);
    }

    public async Task<CityDto?> Find(int cityId)
    {
        var cities = from c in _context.Cities
                     where c.Id == cityId
                     select new CityDto(c.Id, c.Name);

        return await cities.FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyCollection<CityDto>> Read()
    {
        var cities = from c in _context.Cities
                     orderby c.Name
                     select new CityDto(c.Id, c.Name);

        return await cities.ToArrayAsync();
    }

    public async Task<Status> Update(CityDto city)
    {
        var entity = await _context.Cities.FindAsync(city.Id);
        Status status;

        if (entity is null)
        {
            status = NotFound;
        }
        else if (await _context.Cities.FirstOrDefaultAsync(c => c.Id != city.Id && c.Name == city.Name) != null)
        {
            status = Conflict;
        }
        else
        {
            entity.Name = city.Name;
            await _context.SaveChangesAsync();
            status = Updated;
        }

        return status;
    }

    public async Task<Status> Delete(int cityId)
    {
        var city = await _context.Cities.Include(c => c.Characters).FirstOrDefaultAsync(c => c.Id == cityId);
        Status status;

        if (city is null)
        {
            status = NotFound;
        }
        else if (city.Characters.Any())
        {
            status = Conflict;
        }
        else
        {
            _context.Cities.Remove(city);
            await _context.SaveChangesAsync();

            status = Deleted;
        }

        return status;
    }
}
