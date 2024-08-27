using System.Net.Http.Headers;

namespace Mal.Net.Utils;

internal class MalHttpClient : IDisposable
{
    private readonly HttpClient _httpClient = new();
    private readonly string _clientId;

    internal MalHttpClient(string clientId)
    {
        _clientId = clientId;
    }

    public async Task<string> GetAsync(string url, string? token = null)
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

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> PostAsync(string url, HttpContent content, string? token = null)
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