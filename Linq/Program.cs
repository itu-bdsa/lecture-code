using System.Text.RegularExpressions;

var text = File.ReadAllText(args[0]);

var words = Regex.Split(text, @"\P{L}+");

var histogram = from w in words
                group w by w into h
                let c = h.Count()
                orderby c descending
                select new
                {
                    Word = h.Key,
                    Count = c
                };

histogram
    .Take(5)
    .ToList()
    .ForEach(Console.WriteLine);

// var repo = new Repository();

// var heroes0 = repo.Superheroes
//     // .Where(h => h.City == "Gotham City")
//     .OrderByDescending(h => h.FirstAppearance)
//     .ThenBy(h => h.AlterEgo)
//     .Select(h => new { h.AlterEgo, h.FirstAppearance });

// var heroes = from h in repo.Superheroes
//              join c in repo.Cities on h.CityId equals c.Id
//             //  where c.Name == "Gotham City"
//              orderby h.FirstAppearance descending, h.AlterEgo
//              select new { h.AlterEgo, h.FirstAppearance, City = c.Name };

// heroes.Print();

// See https://aka.ms/new-console-template for more information
// Console.WriteLine("Hello, World!");

// Enumerable
//     .Range(0, 100)
//     .Where(x => x % 3 == 0)
//     .Select(x => Math.Sqrt(x))
//     .Print();

// // collection.Select(x => x * 2).ToArray().Print();

// // Func<int, bool> filter = x => x % 2 == 0;

// // Print(System.Linq.Enumerable.Range(0, 100), x => x % 2 == 0);

// Console.WriteLine(Extensions.ToString(new { Name = "Rasmus", Occupation = "Professor" }));

// static void Print<T>(IEnumerable<T> numbers, Func<T, bool> filter)
// {
//     foreach (var x in numbers)
//     {
//         if (filter(x))
//         {
//             Console.WriteLine(x);
//         }
//     }
// }

// static bool Filter2(int x)
// {
//     return x < 3;
// }

// public delegate bool Filter(int x);
