namespace Mal.Net.Utils;
internal static class StringHelper
{
    public static string ToCommaSeparatedString(IEnumerable<string> values)
    {
        return string.Join(",", values);
    }
}
