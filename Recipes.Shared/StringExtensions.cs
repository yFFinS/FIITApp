using System.Globalization;

namespace Recipes.Shared;

public static class StringExtensions
{
    public static string FirstCharToUpper(this string str)
    {
        string HeapFallback()
        {
            var charArray = str.ToCharArray();
            charArray[0] = char.ToUpper(charArray[0]);
            return new string(charArray);
        }

        if (string.IsNullOrWhiteSpace(str))
        {
            return str;
        }

        if (str.Length > 255)
        {
            return HeapFallback();
        }

        Span<char> span = stackalloc char[str.Length];
        if (!str.TryCopyTo(span))
        {
            // Should never happen, but just in case
            throw new InvalidOperationException("Failed to copy string to span");
        }

        span[0] = char.ToUpper(span[0], CultureInfo.InvariantCulture);
        return span.ToString();
    }
}