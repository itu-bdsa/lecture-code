namespace MyApp.Core;

public interface ICharacterRepository
{
    CharacterDetailsDto Create(CharacterCreateDto character);
    CharacterDetailsDto? Find(int characterId);
    IReadOnlyCollection<CharacterDto> Read();
    Status Update(CharacterUpdateDto character);
    Status Delete(int characterId);
}
