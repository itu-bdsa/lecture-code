namespace MyApp.Core;

public interface ICityRepository
{
    CityDto Create(CityCreateDto character);
    CityDto? Find(int CityId);
    IReadOnlyCollection<CityDto> Read();
    bool UpdateAsync(CityDto character);
    bool DeleteAsync(int CityId);
}
