using System.Net;
using System.Net.Http.Headers;
using Mal.Net.Schemas;
using Mal.Net.Exceptions;

namespace Mal.Net.Utils;

internal static class MalHttpClient
{
    private static readonly HttpClient HttpClient = new();
    internal static string ClientId { get; set; } = string.Empty;

    internal static async Task<string> GetAsync(string url, string? exceptionMessage = null, string? token = null)
    {
        HttpClient.DefaultRequestHeaders.Clear();

        if (!string.IsNullOrEmpty(token))
        {
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        else
        {
            HttpClient.DefaultRequestHeaders.Add("X-MAL-CLIENT-ID", ClientId);
        }

        try
        {
            using var response = await HttpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            var errorResponse = await response.Content.ReadAsStringAsync();
            throw new MalHttpException(response.StatusCode, exceptionMessage, malErrorResponse: MalErrorResponse.FromJson(errorResponse));
        }
        catch (HttpRequestException e)
        {
            throw new MalHttpException(HttpStatusCode.ServiceUnavailable, $"Service unavailable: {e.Message}");
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
            throw new MalHttpException(response.StatusCode, exceptionMessage, malErrorResponse: MalErrorResponse.FromJson(errorResponse));
        }
        catch (HttpRequestException e)
        {
            throw new MalHttpException(HttpStatusCode.ServiceUnavailable, $"Service unavailable: {e.Message}");
        }
    }

    public static void Dispose()
    {
        HttpClient.Dispose();
    }
}