namespace MyApp.Infrastructure;

public class PowerRepository : IPowerRepository
{
    private readonly ComicsContext _context;

    public PowerRepository(ComicsContext context)
    {
        _context = context;
    }

    public (Status, PowerDto) Create(PowerCreateDto power)
    {
        var entity = _context.Powers.FirstOrDefault(c => c.Name == power.Name);
        Status status;

        if (entity is null)
        {
            entity = new Power(power.Name);

            _context.Powers.Add(entity);
            _context.SaveChanges();

            status = Created;
        }
        else
        {
            status = Conflict;
        }

        var created = new PowerDto(entity.Id, entity.Name);

        return (status, created);
    }

    public PowerDto? Find(int powerId)
    {
        var cities = from c in _context.Powers
                     where c.Id == powerId
                     select new PowerDto(c.Id, c.Name);

        return cities.FirstOrDefault();
    }

    public IReadOnlyCollection<PowerDto> Read()
    {
        var cities = from c in _context.Powers
                     orderby c.Name
                     select new PowerDto(c.Id, c.Name);

        return cities.ToArray();
    }

    public Status Update(PowerDto power)
    {
        var entity = _context.Powers.Find(power.Id);
        Status status;

        if (entity is null)
        {
            status = NotFound;
        }
        else if (_context.Powers.FirstOrDefault(c => c.Id != power.Id && c.Name == power.Name) != null)
        {
            status = Conflict;
        }
        else
        {
            entity.Name = power.Name;
            _context.SaveChanges();
            status = Updated;
        }

        return status;
    }

    public Status Delete(int powerId)
    {
        var power = _context.Powers.Include(c => c.Characters).FirstOrDefault(c => c.Id == powerId);
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
            _context.SaveChanges();

            status = Deleted;
        }

        return status;
    }
}