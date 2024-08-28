namespace Mal.Net.Exceptions;

public class MalClientException : Exception
{
    public MalClientException(string message) : base(message) { }
    public MalClientException(string message, Exception innerException) : base(message, innerException) { }
}