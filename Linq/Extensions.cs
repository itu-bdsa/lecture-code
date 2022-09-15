namespace Linq;

public static class Extensions
{
    public static int WordCount(this string str) => str.Split(new[] { ' ', '.', '?' }, StringSplitOptions.RemoveEmptyEntries).Length;
}
