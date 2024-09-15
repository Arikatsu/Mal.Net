using System.Net;
using Mal.Net.Schemas;

namespace Mal.Net.Exceptions;

/// <summary>
/// Represents an exception thrown when an HTTP error occurs when interacting with the MyAnimeList API.
/// </summary>
public class MalHttpException : MalClientException
{
    public HttpStatusCode StatusCode { get; }
    public MalErrorResponse? MalErrorResponse { get; }
    public string? AdditionalInfo { get; }

    public MalHttpException(HttpStatusCode statusCode, string? additionalInfo,
        MalErrorResponse? malErrorResponse = null)
        : base(FormatErrorMessage(statusCode, additionalInfo, malErrorResponse))
    {
        StatusCode = statusCode;
        MalErrorResponse = malErrorResponse;
        AdditionalInfo = additionalInfo;
    }

    private static string FormatErrorMessage(HttpStatusCode statusCode, string? additionalInfo,
        MalErrorResponse? errorResponse)
    {
        var errorMessage = $"Failed to fetch resource.\n";

        if (!string.IsNullOrEmpty(additionalInfo))
        {
            errorMessage += $" {additionalInfo}";
        }

        if (errorResponse == null) return errorMessage;

        var message = !string.IsNullOrEmpty(errorResponse.Message)
            ? errorResponse.Message
            : statusCode switch
            {
                HttpStatusCode.NotFound => "Resource not found",
                HttpStatusCode.BadRequest => "Invalid parameters provided",
                HttpStatusCode.Forbidden => "Forbidden access",
                _ => "An error occurred"
            };
            
        errorMessage += $" - {message} (Status Code: {(int)statusCode})";

        return errorMessage;
    }

    public override string ToString()
    {
        var baseInfo = base.ToString();
        var errorInfo = $"Exception occurred at {baseInfo}";

        return $"{Message}\n{errorInfo}";
    }
}