using System.Text.RegularExpressions;

namespace Mal.Net.Utils;

internal static class StringHelper
{
    internal static string ToCommaSeparatedString(IEnumerable<string> values)
    {
        return string.Join(",", values);
    }
}

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
public class ValidParams : Attribute
{
    private readonly string _pattern;
    
    public ValidParams(params string[] values)
    {
        var escapedValues = values.Select(Regex.Escape);
        _pattern = $"^({string.Join("|", escapedValues)})$";
    }
    
    
}
