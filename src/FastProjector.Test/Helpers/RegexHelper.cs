using System.Text.RegularExpressions;

namespace FastProjector.Test.Helpers;

public static class RegexHelper
{
    public const string AnyNamespace = @"(\w+\.)*";
    public const string Anything = @".*";
    
    public static string ReplaceSpaceWithAnySpace(this string text)
    {
        return Regex.Replace(text, @"\s+",@"\s*");
    }
}   