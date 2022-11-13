namespace MyApp.Core;

public interface ICityRepository
{
    Task<Results<Created<City>, ValidationProblem, Conflict<City>>> CreateAsync(City city);
    Task<Results<Ok<City>, NotFound<int>>> FindAsync(int id);
    Task<IReadOnlyCollection<City>> ReadAsync();
    Task<Results<NoContent, ValidationProblem, NotFound<int>, Conflict<City>>> UpdateAsync(int id, City city);
    Task<Results<NoContent, NotFound<int>, Conflict<City>>> DeleteAsync(int id);
}
