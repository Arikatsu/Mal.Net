using System.Text;
using Xunit.Abstractions;

namespace Mal.Net.Testing;

internal class LogConverter : TextWriter
{
    private readonly ITestOutputHelper _output;

    internal LogConverter(ITestOutputHelper output)
    {
        _output = output;
    }

    public override Encoding Encoding => Encoding.Default;

    public override void WriteLine(string? message)
    {
        _output.WriteLine(message);
    }

    public override void WriteLine(string message, params object?[]? args)
    {
        _output.WriteLine(message, args);
    }

    public override void Write(char value)
    {
        throw new NotSupportedException(
            "This text writer only supports WriteLine(string) and WriteLine(string, params object[]).");
    }
}