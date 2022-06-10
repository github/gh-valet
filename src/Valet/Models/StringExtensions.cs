namespace Valet.Models;

public static class StringExtensions
{
    public static string EscapeIfNeeded(this string str)
    {
        if (!str.Contains(' '))
            return str;
        
        return $"\"{str}\"";
    }
}