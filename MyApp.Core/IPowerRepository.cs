namespace MyApp.Core;

public interface IPowerRepository
{
    PowerDto Create(PowerCreateDto character);
    PowerDto? Find(int powerId);
    IReadOnlyCollection<PowerDto> Read();
    bool UpdateAsync(PowerDto character);
    bool DeleteAsync(int powerId);
}
