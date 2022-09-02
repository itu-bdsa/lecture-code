namespace HelloWorld.Tests;

public class ProgramTests
{
    [Fact]
    public void Main_when_run_prints_Hello_World()
    {
        // Arrange
        using var writer = new StringWriter();
        Console.SetOut(writer);

        // Act
        Program.Main(null);

        // Assert

    }
}