namespace Linq;

public static class Extensions
{
    public static int WordCount(this string str) =>
        str.Split(new[] { ' ', '.', '?' }, StringSplitOptions.RemoveEmptyEntries).Length;


    public static void Print<T>(this IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            Console.WriteLine(item);
        }
    }

    public static string ToString<T>(this T stuff)
    {
        return "foo";
    }
}
