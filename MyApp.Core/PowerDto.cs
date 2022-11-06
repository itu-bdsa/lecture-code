namespace MyApp.Core;

public record PowerCreateDto([StringLength(50, MinimumLength = 1)] string Name);
public record PowerDto(int Id, [StringLength(50, MinimumLength = 1)] string Name) : PowerCreateDto(Name);
