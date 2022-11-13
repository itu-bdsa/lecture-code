using FluentValidation;

namespace MyApp.Core;

public sealed record Power
{
    public int Id { get; init; }

    public required string Name { get; init; }
}

public sealed class PowerValidator : AbstractValidator<Power>
{
    public PowerValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
    }
}
