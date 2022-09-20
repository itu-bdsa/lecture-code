namespace MyApp.Core
{
    public record CityCreateDto([StringLength(50)] string Name);
    public record CityDto(int Id, [StringLength(50)] string Name) : CityCreateDto(Name);
}
