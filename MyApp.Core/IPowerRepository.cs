namespace MyApp.Core;

public interface IPowerRepository
{
    (Status, PowerDto) Create(PowerCreateDto power);
    PowerDto? Find(int powerId);
    IReadOnlyCollection<PowerDto> Read();
    Status Update(PowerDto power);
    Status Delete(int powerId);
}
