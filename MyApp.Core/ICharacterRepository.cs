namespace MyApp.Core;

public interface ICharacterRepository
{
    Task<CharacterDetailsDto> CreateAsync(CharacterCreateDto character);
    Task<CharacterDetailsDto?> FindAsync(int characterId);
    Task<IReadOnlyCollection<CharacterDto>> ReadAsync();
    Task<Status> UpdateAsync(CharacterUpdateDto character);
    Task<Status> DeleteAsync(int characterId);
}
