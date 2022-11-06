namespace MyApp.Client.Tests.Pages;

public class CounterTests
{
    [Fact]
    public void CounterShouldIncrementWhenSelected()
    {
        // Arrange
        using var context = new TestContext();
        var component = context.RenderComponent<Counter>();
        var p = component.Find("p");

        // Act
        component.Find("button").Click();

        // Assert
        p.TextContent.MarkupMatches("Current count: 1");
    }
}