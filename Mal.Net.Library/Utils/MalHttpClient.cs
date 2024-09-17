using System.Net;
using System.Net.Http.Headers;
using Mal.Net.Schemas;
using Mal.Net.Exceptions;

namespace Mal.Net.Utils;

internal static class MalHttpClient
{
    private static readonly object Lock = new();
    private static readonly HttpClient HttpClient = new();
    private static readonly SemaphoreSlim Semaphore = new(1, 1);

    private static string? _clientId;

    internal static void SetClientId(string clientId)
    {
        lock (Lock)
        {
            _clientId = clientId;
        }
    }

    internal static async Task<string> GetAsync(string url, string? exceptionMessage = null, string? tokenType = null,
        string? token = null)
    {
        await Semaphore.WaitAsync();
        
        HttpClient.DefaultRequestHeaders.Clear();

        if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(tokenType))
        {
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, token);
        }
        else if (!string.IsNullOrEmpty(_clientId))
        {
            HttpClient.DefaultRequestHeaders.Add("X-MAL-CLIENT-ID", _clientId);
        }
        else
        {
            throw new MalHttpException(HttpStatusCode.BadRequest, "Client ID not set");
        }

        try
        {
            using var response = await HttpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            var errorResponse = await response.Content.ReadAsStringAsync();
            throw new MalHttpException(response.StatusCode, exceptionMessage, MalErrorResponse.FromJson(errorResponse));
        }
        catch (HttpRequestException e)
        {
            throw new MalHttpException(HttpStatusCode.ServiceUnavailable, $"Service unavailable: {e.Message}");
        }
        finally
        {
            Semaphore.Release();
        }
    }

    internal static async Task<string> PostAsync(string url, HttpContent content, string? exceptionMessage = null)
    {
        content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

        var contentString = await content.ReadAsStringAsync();
        Console.WriteLine($"Request Content: {contentString}");

        try
        {
            using var response = await HttpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            var errorResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error Response: {errorResponse}");
            throw new MalHttpException(response.StatusCode, exceptionMessage, MalErrorResponse.FromJson(errorResponse));
        }
        catch (HttpRequestException e)
        {
            throw new MalHttpException(HttpStatusCode.ServiceUnavailable, $"Service unavailable: {e.Message}");
        }
    }
}