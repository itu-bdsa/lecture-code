namespace Linq;

public class LocalFunctions
{
    int LocalFunctionFactorial(int n)
    {
        int Factorial(int number)
        {
            if (number < 2)
            {
                return 1;
            }
            else
            {
                return number * Factorial(number - 1);
            }
        }

        return Factorial(n);
    }

    int LambdaFactorial(int n)
    {
        Func<int, int> nthFactorial = default(Func<int, int>);

        nthFactorial = number => number < 2
            ? 1
            : number * nthFactorial(number - 1);

        return nthFactorial(n);
    }
}