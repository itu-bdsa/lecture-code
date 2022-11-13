namespace MyApp.Infrastructure;

public sealed class CharacterRepository : ICharacterRepository
{
    private readonly ComicsContext _context;
    private readonly CharacterValidator _validator;

    public CharacterRepository(ComicsContext context, CharacterValidator validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<Results<Created<Character>, ValidationProblem>> CreateAsync(Character character)
    {
        var validation = _validator.Validate(character);

        if (!validation.IsValid)
        {
            return TypedResults.ValidationProblem(validation.ToDictionary());
        }

        var entity = new CharacterEntity
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

        return TypedResults.Created($"{entity.Id}", character with { Id = entity.Id });
    }

    public async Task<Results<Ok<Character>, NotFound<int>>> FindAsync(int id)
    {
        var characters = from c in _context.Characters
                         where c.Id == id
                         select new Character
                         {
                             Id = c.Id,
                             AlterEgo = c.AlterEgo,
                             GivenName = c.GivenName,
                             Surname = c.Surname,
                             FirstAppearance = c.FirstAppearance,
                             Occupation = c.Occupation,
                             Gender = c.Gender,
                             City = c.City!.Name,
                             ImageUrl = c.ImageUrl,
                             Powers = c.Powers.Select(p => p.Name).ToHashSet()
                         };

        var character = await characters.FirstOrDefaultAsync();

        return character is null ? TypedResults.NotFound(id) : TypedResults.Ok(character);
    }

    public async Task<IReadOnlyCollection<BasicCharacter>> ReadAsync()
    {
        var characters = from c in _context.Characters
                         orderby c.AlterEgo, c.GivenName, c.Surname
                         select new BasicCharacter
                         {
                             Id = c.Id,
                             AlterEgo = c.AlterEgo,
                             GivenName = c.GivenName,
                             Surname = c.Surname
                         };

        return await characters.ToListAsync();
    }

    public async Task<Results<NoContent, ValidationProblem, NotFound<int>>> UpdateAsync(int id, Character character)
    {
        var entity = await _context.Characters.FindAsync(character.Id);

        if (entity == null)
        {
            return TypedResults.NotFound(id);
        }

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

        return TypedResults.NoContent();
    }

    public async Task<Results<NoContent, NotFound<int>>> DeleteAsync(int id)
    {
        var entity = await _context.Characters.FindAsync(id);

        if (entity == null)
        {
            return TypedResults.NotFound(id);
        }

        _context.Characters.Remove(entity);
        await _context.SaveChangesAsync();

        return TypedResults.NoContent();
    }

    private async Task<CityEntity?> CreateOrUpdateCity(string? cityName) => string.IsNullOrWhiteSpace(cityName) ? null : await _context.Cities.FirstOrDefaultAsync(c => c.Name == cityName) ?? new CityEntity { Name = cityName };

    private async IAsyncEnumerable<PowerEntity> CreateOrUpdatePowers(IEnumerable<string> powerNames)
    {
        var existing = await _context.Powers.Where(p => powerNames.Contains(p.Name)).ToDictionaryAsync(p => p.Name);

        foreach (var powerName in powerNames)
        {
            existing.TryGetValue(powerName, out var power);

            yield return power ?? new PowerEntity { Name = powerName };
        }
    }
}
