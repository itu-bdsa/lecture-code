namespace Generics;

public class Person
{
}

public sealed class Student : Person
{
    public int Semester { get; set; }
}

public sealed class PeopleComparer : IComparer<Person>
{
    public int Compare(Person? x, Person? y) => throw new NotImplementedException();
}

public sealed class StudentComparer : IComparer<Student>
{
    public int Compare(Student? x, Student? y) => x?.Semester - y?.Semester ?? 0;
}

public sealed class Variance
{
    public void ArrayTypeCovariance()
    {
        Person[] people = new Student[9];

        // What happens here?
        people[3] = new Person();
    }

    public void Invariance()
    {
        // List<object> obj;

        // IList<Person> people = new List<Student>();

        // IList<Student> students = new List<Person>();
    }

    public void Covariance()
    {
        // IEnumerable<object> obj;

        // IEnumerable<Person> people = new List<Student>();

        // IEnumerable<Student> students = new List<Person>();
    }

    public void Contravariance()
    {
        // IComparer<object> objComparer;
        // IComparable<object> objComparable;
        // Comparison<object> objComparison;

        // IComparer<Person> studentComparer = new StudentComparer();

        // IComparer<Student> peopleComparer = new PeopleComparer();
    }
}
