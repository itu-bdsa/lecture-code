namespace MyApp.Infrastructure;

public class CityRepository : ICityRepository
{
    private readonly ComicsContext _context;

    public CityRepository(ComicsContext context)
    {
        _context = context;
    }

    public (Status, CityDto) Create(CityCreateDto city)
    {
        var entity = _context.Cities.FirstOrDefault(c => c.Name == city.Name);
        Status status;

        if (entity is null)
        {
            entity = new City(city.Name);

            _context.Cities.Add(entity);
            _context.SaveChanges();

            status = Created;
        }
        else
        {
            status = Conflict;
        }

        var created = new CityDto(entity.Id, entity.Name);

        return (status, created);
    }

    public CityDto? Find(int cityId)
    {
        var cities = from c in _context.Cities
                     where c.Id == cityId
                     select new CityDto(c.Id, c.Name);

        return cities.FirstOrDefault();
    }

    public IReadOnlyCollection<CityDto> Read()
    {
        var cities = from c in _context.Cities
                     orderby c.Name
                     select new CityDto(c.Id, c.Name);

        return cities.ToArray();
    }

    public Status Update(CityDto city)
    {
        var entity = _context.Cities.Find(city.Id);
        Status status;

        if (entity is null)
        {
            status = NotFound;
        }
        else if (_context.Cities.FirstOrDefault(c => c.Id != city.Id && c.Name == city.Name) != null)
        {
            status = Conflict;
        }
        else
        {
            entity.Name = city.Name;
            _context.SaveChanges();
            status = Updated;
        }

        return status;
    }

    public Status Delete(int cityId)
    {
        var city = _context.Cities.Include(c => c.Characters).FirstOrDefault(c => c.Id == cityId);
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
            _context.SaveChanges();

            status = Deleted;
        }

        return status;
    }
}