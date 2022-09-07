namespace Generics;

public static class PostalCodeValidator
{
    public static bool IsValid(string postalCode)
    {
        return false;
    }

    public static bool TryParse(string postalCodeAndLocality,
        out string postalCode,
        out string locality)
    {
        postalCode = string.Empty;
        locality = string.Empty;
        return false;
    }
}