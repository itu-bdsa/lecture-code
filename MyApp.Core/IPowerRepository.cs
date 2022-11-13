namespace MyApp.Core;

public interface IPowerRepository
{
    Task<Results<Created<Power>, ValidationProblem, Conflict<Power>>> CreateAsync(Power power);
    Task<Results<Ok<Power>, NotFound<int>>> FindAsync(int id);
    Task<IReadOnlyCollection<Power>> ReadAsync();
    Task<Results<NoContent, ValidationProblem, NotFound<int>, Conflict<Power>>> UpdateAsync(int id, Power power);
    Task<Results<NoContent, NotFound<int>, Conflict<Power>>> DeleteAsync(int id);
}
