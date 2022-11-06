namespace MyApp.Core;

public record CityCreateDto([StringLength(50, MinimumLength = 1)] string Name);
public record CityDto(int Id, [StringLength(50, MinimumLength = 1)] string Name) : CityCreateDto(Name);
