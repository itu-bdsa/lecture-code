namespace HelloWorld;

public class Calculator
{
    public int Add(string numbers)
    {
        var result = 0;

        foreach (var number in numbers.Split(' '))
        {
            var num = int.Parse(number);

            result += num;
        }

        return result;
    }
}