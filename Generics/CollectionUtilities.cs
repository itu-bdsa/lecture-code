namespace Generics;

public class CollectionUtilities
{
    public static IEnumerable<int> GetEven(IEnumerable<int> list, int stopMax = int.MaxValue)
    {
        foreach (var v in list)
        {
            if (v >= stopMax)
            {
                yield break;
            }

            if (v % 2 == 0)
            {
                yield return v;
            }
        }
    }

    public static bool Find(int[] list, int number)
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<int> Unique(IEnumerable<int> numbers)
        => new HashSet<int>(numbers);

    public static IEnumerable<int> Reverse(IEnumerable<int> numbers)
        => new Stack<int>(numbers);

    public static void Sort<T>(List<T> items, IComparer<T>? comparer = null)
    {
        items.Sort(comparer);
    }

    public static IDictionary<int, Duck> ToDictionary(IEnumerable<Duck> ducks)
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Duck> GetOlderThan(IEnumerable<Duck> ducks, int age)
    {
        throw new NotImplementedException();
    }
}