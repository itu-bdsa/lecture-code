var ducks = Duck.Ducks.ToList();
CollectionUtilities.Sort(ducks, new DuckAgeComparer());

foreach (var duck in ducks)
{
    Console.WriteLine(duck);
}