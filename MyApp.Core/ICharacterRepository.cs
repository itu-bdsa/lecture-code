namespace MyApp.Core;

public interface ICharacterRepository
{
    Task<(Status, CharacterDetailsDto)> CreateAsync(CharacterCreateDto character);
    Task<CharacterDetailsDto?> FindAsync(int characterId);
    Task<IReadOnlyCollection<CharacterDto>> Read();
    Task<Status> UpdateAsync(CharacterUpdateDto character);
    Task<Status> DeleteAsync(int characterId);
}
