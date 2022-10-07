namespace MyApp.Core;

public interface IPowerRepository
{
    Task<(Status, PowerDto)> Create(PowerCreateDto power);
    Task<PowerDto?> Find(int powerId);
    Task<IReadOnlyCollection<PowerDto>> Read();
    Task<Status> Update(PowerDto power);
    Task<Status> Delete(int powerId);
}
