using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mal.Net.Models;

public class MalErrorResponse
{
    [JsonPropertyName("error")]
    public string? Error { get; set; } = "Unknown";
    
    [JsonPropertyName("message")]
    public string? Message { get; set; } = "An error occurred while processing the request";
    
    public static MalErrorResponse FromJson(string json)
    {
        return JsonSerializer.Deserialize<MalErrorResponse>(json) ?? new MalErrorResponse();
    }
}