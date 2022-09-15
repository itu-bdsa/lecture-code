// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

Filter filter = new Filter(
    delegate (int x)
    {
        return x % 2 == 0;
    }
);

Print(System.Linq.Enumerable.Range(0, 100), filter);

static void Print(IEnumerable<int> numbers, Filter filter)
{
    foreach (var x in numbers)
    {
        if (filter(x))
        {
            Console.WriteLine(x);
        }
    }
}

public delegate bool Filter(int x);