namespace MyApp.Infrastructure;

public sealed class CharacterRepository : ICharacterRepository
{
    private readonly ComicsContext _context;

    public CharacterRepository(ComicsContext context)
    {
        _context = context;
    }

    public (Status, CharacterDetailsDto) Create(CharacterCreateDto character)
    {
        var entity = new Character
        {
            AlterEgo = character.AlterEgo,
            GivenName = character.GivenName,
            Surname = character.Surname,
            FirstAppearance = character.FirstAppearance,
            Occupation = character.Occupation,
            Gender = character.Gender,
            City = CreateOrUpdateCity(character.City),
            ImageUrl = character.ImageUrl,
            Powers = CreateOrUpdatePowers(character.Powers).ToHashSet()
        };

        _context.Characters.Add(entity);
        _context.SaveChanges();

        return (Created, new CharacterDetailsDto(entity.Id, entity.AlterEgo, entity.GivenName, entity.Surname, entity.FirstAppearance, entity.Occupation, entity.City?.Name, entity.Gender, entity.ImageUrl, entity.Powers.Select(p => p.Name).ToHashSet()));
    }

    public CharacterDetailsDto? Find(int characterId)
    {
        var characters = from c in _context.Characters
                         let powers = c.Powers.Select(p => p.Name).ToHashSet()
                         where c.Id == characterId
                         select new CharacterDetailsDto(c.Id, c.AlterEgo, c.GivenName, c.Surname, c.FirstAppearance, c.Occupation, c.City == null ? null : c.City.Name, c.Gender, c.ImageUrl, powers);

        return characters.FirstOrDefault();
    }

    public IReadOnlyCollection<CharacterDto> Read()
    {
        var characters = from c in _context.Characters
                         select new CharacterDto(c.Id, c.AlterEgo, c.GivenName, c.Surname);

        return characters.ToList();
    }

    public Status Update(CharacterUpdateDto character)
    {
        var entity = _context.Characters.Find(character.Id);

        if (entity == null)
        {
            return NotFound;
        }

        entity.AlterEgo = character.AlterEgo;
        entity.GivenName = character.GivenName;
        entity.Surname = character.Surname;
        entity.FirstAppearance = character.FirstAppearance;
        entity.Occupation = character.Occupation;
        entity.Gender = character.Gender;
        entity.City = CreateOrUpdateCity(character.City);
        entity.ImageUrl = character.ImageUrl;
        entity.Powers = CreateOrUpdatePowers(character.Powers).ToHashSet();

        _context.SaveChanges();

        return Updated;
    }

    public Status Delete(int characterId)
    {
        var entity = _context.Characters.Find(characterId);

        if (entity == null)
        {
            return NotFound;
        }

        _context.Characters.Remove(entity);
        _context.SaveChanges();

        return Deleted;
    }

    private City? CreateOrUpdateCity(string? cityName) => cityName is null ? null : _context.Cities.FirstOrDefault(c => c.Name == cityName) ?? new City(cityName);

    private IEnumerable<Power> CreateOrUpdatePowers(IEnumerable<string> powerNames)
    {
        var existing = _context.Powers.Where(p => powerNames.Contains(p.Name)).ToDictionary(p => p.Name);

        foreach (var powerName in powerNames)
        {
            existing.TryGetValue(powerName, out var power);

            yield return power ?? new Power(powerName);
        }
    }
}
