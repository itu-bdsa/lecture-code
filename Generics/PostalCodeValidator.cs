namespace Generics;

using System.Text.RegularExpressions;

public static class PostalCodeValidator
{
    public static bool IsValid(string postalCode) => Regex.IsMatch(postalCode, @"^\d{4}$");

    public static bool TryParse(string postalCodeAndLocality,
        out string postalCode,
        out string locality)
    {
        var pattern = @"^(?<postal_code>\d{4}) (?<locality>.+)$";

        var match = Regex.Match(postalCodeAndLocality, pattern);

        if (match.Success)
        {
            postalCode = match.Groups["postal_code"].Value;
            locality = match.Groups["locality"].Value;
            return true;
        }
        else
        {
            postalCode = string.Empty;
            locality = string.Empty;
            return false;
        }
    }

    public static (bool valid, string postalCode, string locality) TryParse(string postalCodeAndLocality)
    {
        var pattern = @"^(?<postal_code>\d{4}) (?<locality>.+)$";

        var match = Regex.Match(postalCodeAndLocality, pattern);

        var postalCode = string.Empty;
        var locality = string.Empty;

        if (match.Success)
        {
            postalCode = match.Groups["postal_code"].Value;
            locality = match.Groups["locality"].Value;
        }

        return (match.Success, postalCode, locality);
    }
}