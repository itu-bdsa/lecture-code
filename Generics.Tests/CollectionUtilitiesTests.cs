namespace Generics.Tests;

public class CollectionUtilitiesTests
{
    [Fact]
    public void GetEven_given_1_to_5_returns_2_and_4()
    {
        // var list = new List<int> { 4, 6, 7 };
        int[] list = { 1, 2, 3, 4, 5 };

        // Act
        var evens = CollectionUtilities.GetEven(list);

        // Assert
        evens.Should().BeEquivalentTo(new[] { 2, 4 });
    }

    [Fact]
    public void GetEven_given_1_to_5_and_stopMax_3_returns_2()
    {
        // Arrange
        int[] list = { 1, 2, 3, 4, 5 };

        // Act
        var evens = CollectionUtilities.GetEven(list, stopMax: 3);

        // Assert
        evens.Should().BeEquivalentTo(new[] { 2 });
    }

    [Fact]
    public void Unique_given_1_2_2_3_2_returns_1_2_3()
    {
        // Arrange
        int[] list = { 1, 2, 2, 3, 2 };

        // Act
        var unique = CollectionUtilities.Unique(list);

        // Assert
        unique.Should().BeEquivalentTo(new[] { 1, 2, 3 });
    }

    [Fact]
    public void Reverse_given_4_3_2_1_returns_1_2_3_4()
    {
        // Arrange
        int[] list = { 4, 3, 2, 1 };

        // Act
        var reversed = CollectionUtilities.Reverse(list);

        // Assert
        Assert.Equal(new[] { 1, 2, 3, 4 }, reversed);
    }
}