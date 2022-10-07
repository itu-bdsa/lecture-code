namespace MyApp.Core;

public interface ICharacterRepository
{
    Task<(Status, CharacterDetailsDto)> Create(CharacterCreateDto character);
    Task<CharacterDetailsDto?> Find(int characterId);
    Task<IReadOnlyCollection<CharacterDto>> Read();
    Task<Status> Update(CharacterUpdateDto character);
    Task<Status> Delete(int characterId);
}
