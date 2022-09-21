namespace MyApp.Core;

public interface ICityRepository
{
    (Status, CityDto) Create(CityCreateDto city);
    CityDto? Find(int cityId);
    IReadOnlyCollection<CityDto> Read();
    Status Update(CityDto city);
    Status Delete(int cityId);
}
