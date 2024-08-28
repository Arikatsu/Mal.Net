namespace Mal.Net.Utils;
internal static class StringHelper
{
    internal static string ToCommaSeparatedString(IEnumerable<string> values)
    {
        return string.Join(",", values);
    }
}
