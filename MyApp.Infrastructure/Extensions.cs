namespace MyApp.Infrastructure;

public static class Extensions
{
    public static async Task<HashSet<T>> ToHashSetAsync<T>(this IAsyncEnumerable<T> items)
    {
        var results = new HashSet<T>();

        await foreach (var item in items)
        {
            results.Add(item);
        }

        return results;
    }
}