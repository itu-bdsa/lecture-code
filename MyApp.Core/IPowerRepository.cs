namespace MyApp.Core;

public interface IPowerRepository
{
    Task<(Status, PowerDto)> CreateAsync(PowerCreateDto power);
    Task<PowerDto?> FindAsync(int powerId);
    Task<IReadOnlyCollection<PowerDto>> ReadAsync();
    Task<Status> UpdateAsync(PowerDto power);
    Task<Status> DeleteAsync(int powerId);
}
