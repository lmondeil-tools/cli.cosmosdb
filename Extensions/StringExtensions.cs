namespace lmondeil.cli.template.Extensions;
using System.Text.RegularExpressions;

internal static class StringExtensions
{
    public static string RegexReplace(this string @this, string pattern, string replacement, RegexOptions options = RegexOptions.Multiline)
    {
        return Regex.Replace(@this, pattern, replacement, options);
    }
    public static string RegexReplace(this string @this, string pattern, MatchEvaluator matchEvaluator)
    {
        return Regex.Replace(@this, pattern, matchEvaluator);
    }
}
