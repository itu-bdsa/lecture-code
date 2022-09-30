namespace Generics;

using static TrafficLightColor;

public sealed class TrafficLightController : ITrafficLightController
{
    public bool MayIGo(TrafficLightColor color) => color switch
    {
        Red => false,
        Yellow => false,
        Green => true,
        _ => throw new ArgumentException("Invalid color", nameof(color))
    };
}