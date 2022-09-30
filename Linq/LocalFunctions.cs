namespace Linq;

public sealed class LocalFunctions
{
    public int Factorial(int number) => number < 2
        ? 1
        : number * Factorial(number - 1);

    public int LambdaFactorial(int n)
    {
        Func<int, int> nthFactorial = default!;

        nthFactorial = number => number < 2
            ? 1
            : number * nthFactorial(number - 1);

        return nthFactorial(n);
    }
}