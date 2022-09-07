namespace Generics;

public class TrafficLightController : ITrafficLightController
{
    public bool MayIGo(string color)
    {
        color = color.ToLowerInvariant();

        if (color == "green")
        {
            return true;
        }

        if (color == "yellow" || color == "red")
        {
            return false;
        }

        throw new ArgumentException("Invalid color", nameof(color));
    }
}