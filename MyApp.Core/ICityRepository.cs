namespace MyApp.Core;

public interface ICityRepository
{
    Task<(Status, CityDto)> Create(CityCreateDto city);
    Task<CityDto?> Find(int cityId);
    Task<IReadOnlyCollection<CityDto>> Read();
    Task<Status> Update(CityDto city);
    Task<Status> Delete(int cityId);
}
