using FluentValidation;

namespace MyApp.Core;

public sealed record City
{
    public int Id { get; init; }

    public required string Name { get; init; }
}

public sealed class CityValidator : AbstractValidator<City>
{
    public CityValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
    }
}
