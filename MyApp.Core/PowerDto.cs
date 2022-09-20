namespace MyApp.Core
{
    public record PowerCreateDto([StringLength(50)] string Name);
    public record PowerDto(int Id, [StringLength(50)] string Name) : PowerCreateDto(Name);
}
