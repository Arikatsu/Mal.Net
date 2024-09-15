using System.Net;
using System.Net.Http.Headers;
using Mal.Net.Schemas;
using Mal.Net.Exceptions;

namespace Mal.Net.Utils;

internal class MalHttpClient : IDisposable
{
    private readonly HttpClient _httpClient = new();
    private readonly string _clientId;

    internal MalHttpClient(string clientId)
    {
        _clientId = clientId;
    }

    internal async Task<string> GetAsync(string url, string? exceptionMessage = null, string? token = null)
    {
        _httpClient.DefaultRequestHeaders.Clear();

        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        else
        {
            _httpClient.DefaultRequestHeaders.Add("X-MAL-CLIENT-ID", _clientId);
        }

        try
        {
            using var response = await _httpClient.GetAsync(url);

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


    internal async Task<string> PostAsync(string url, HttpContent content, string? token = null)
    {
        _httpClient.DefaultRequestHeaders.Clear();

        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        else
        {
            _httpClient.DefaultRequestHeaders.Add("X-MAL-CLIENT-ID", _clientId);
        }

        var response = await _httpClient.PostAsync(url, content);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }
}