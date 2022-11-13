namespace MyApp.Core;

public interface ICharacterRepository
{
    Task<Results<Created<Character>, ValidationProblem>> CreateAsync(Character character);
    Task<Results<Ok<Character>, NotFound<int>>> FindAsync(int id);
    Task<IReadOnlyCollection<BasicCharacter>> ReadAsync();
    Task<Results<NoContent, ValidationProblem, NotFound<int>>> UpdateAsync(int id, Character character);
    Task<Results<NoContent, NotFound<int>>> DeleteAsync(int id);
}
