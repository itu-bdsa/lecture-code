# Notes

```csharp
public static void Hamlet()
{
    var text = File.ReadAllText(args[0]);

    var words = Regex.Split(text, @"\P{L}+");

    var histogram = from w in words
                    group w by w into h
                    let c = h.Count()
                    orderby c descending
                    select new { Word = h.Key, Count = c };

    histogram.Take(5).ToList().ForEach(Console.WriteLine);
}
```
