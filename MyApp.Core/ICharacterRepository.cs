namespace MyApp.Core;

public interface ICharacterRepository
{
    CharacterDetailsDto Create(CharacterCreateDto character);
    CharacterDetailsDto? Find(int characterId);
    IReadOnlyCollection<CharacterDto> Read();
    bool UpdateAsync(CharacterUpdateDto character);
    bool DeleteAsync(int characterId);
}
