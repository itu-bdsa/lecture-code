namespace Generics.Tests;

public sealed class TrafficLightControllerTests
{
    [Theory]
    [InlineData(TrafficLightColor.Red, false)]
    [InlineData(TrafficLightColor.Yellow, false)]
    [InlineData(TrafficLightColor.Green, true)]
    public void MayIGo_given_color_returns_go(TrafficLightColor color, bool go)
    {
        var ctrl = new TrafficLightController();

        ctrl.MayIGo(color).Should().Be(go);
    }
}