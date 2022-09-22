namespace HelloWorld.Tests;

public sealed class CalculatorTests
{
    [Fact]
    public void Add_given_1_2_3_returns_6()
    {
        // Arrange
        var sut = new Calculator();

        // Act
        var result = sut.Add("1 2 3");

        // Assert
        result.Should().Be(6);
    }
}