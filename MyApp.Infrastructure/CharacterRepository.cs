namespace MyApp.Infrastructure;

public sealed class CharacterRepository : ICharacterRepository
{
    private readonly ComicsContext _context;

    public CharacterRepository(ComicsContext context)
    {
        _context = context;
    }

    public async Task<(Status, CharacterDetailsDto)> Create(CharacterCreateDto character)
    {
        var entity = new Character
        {
            AlterEgo = character.AlterEgo,
            GivenName = character.GivenName,
            Surname = character.Surname,
            FirstAppearance = character.FirstAppearance,
            Occupation = character.Occupation,
            Gender = character.Gender,
            City = await CreateOrUpdateCity(character.City),
            ImageUrl = character.ImageUrl,
            Powers = await CreateOrUpdatePowers(character.Powers).ToHashSetAsync()
        };

        _context.Characters.Add(entity);
        await _context.SaveChangesAsync();

        return (Created, new CharacterDetailsDto(entity.Id, entity.AlterEgo, entity.GivenName, entity.Surname, entity.FirstAppearance, entity.Occupation, entity.City?.Name, entity.Gender, entity.ImageUrl, entity.Powers.Select(p => p.Name).ToHashSet()));
    }

    public async Task<CharacterDetailsDto?> Find(int characterId)
    {
        var characters = from c in _context.Characters
                         let powers = c.Powers.Select(p => p.Name).ToHashSet()
                         where c.Id == characterId
                         select new CharacterDetailsDto(c.Id, c.AlterEgo, c.GivenName, c.Surname, c.FirstAppearance, c.Occupation, c.City == null ? null : c.City.Name, c.Gender, c.ImageUrl, powers);

        return await characters.FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyCollection<CharacterDto>> Read()
    {
        var characters = from c in _context.Characters
                         select new CharacterDto(c.Id, c.AlterEgo, c.GivenName, c.Surname);

        return await characters.ToListAsync();
    }

    public async Task<Status> Update(CharacterUpdateDto character)
    {
        var entity = await _context.Characters.FindAsync(character.Id);

        if (entity == null)
        {
            return NotFound;
        }

        var powers = CreateOrUpdatePowers(character.Powers);

        entity.AlterEgo = character.AlterEgo;
        entity.GivenName = character.GivenName;
        entity.Surname = character.Surname;
        entity.FirstAppearance = character.FirstAppearance;
        entity.Occupation = character.Occupation;
        entity.Gender = character.Gender;
        entity.City = await CreateOrUpdateCity(character.City);
        entity.ImageUrl = character.ImageUrl;
        entity.Powers = await CreateOrUpdatePowers(character.Powers).ToHashSetAsync();

        await _context.SaveChangesAsync();

        return Updated;
    }

    public async Task<Status> Delete(int characterId)
    {
        var entity = await _context.Characters.FindAsync(characterId);

        if (entity == null)
        {
            return NotFound;
        }

        _context.Characters.Remove(entity);
        await _context.SaveChangesAsync();

        return Deleted;
    }

    private async Task<City?> CreateOrUpdateCity(string? cityName) => string.IsNullOrWhiteSpace(cityName) ? null : await _context.Cities.FirstOrDefaultAsync(c => c.Name == cityName) ?? new City(cityName);

    private async IAsyncEnumerable<Power> CreateOrUpdatePowers(IEnumerable<string> powerNames)
    {
        var existing = await _context.Powers.Where(p => powerNames.Contains(p.Name)).ToDictionaryAsync(p => p.Name);

        foreach (var powerName in powerNames)
        {
            existing.TryGetValue(powerName, out var power);

            yield return power ?? new Power(powerName);
        }
    }
}
