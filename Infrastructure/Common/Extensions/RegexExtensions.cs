using System.Text.RegularExpressions;

namespace Auth1796.Infrastructure.Common.Extensions;

public static class RegexExtensions
{
    private static readonly Regex Whitespace = new(@"\s+");

    public static string ReplaceWhitespace(this string input, string replacement)
    {
        return Whitespace.Replace(input, replacement);
    }
}