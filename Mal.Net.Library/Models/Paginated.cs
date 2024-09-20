using System.Text.Json;
using System.Text.Json.Serialization;
using Mal.Net.Utils;

namespace Mal.Net.Models;

public class Paginated<T>
{
    private static readonly JsonSerializerOptions Options = new() { Converters = { new SingleDataConverter<T>() } };
    
    # region Json Properties
    
    [JsonPropertyName("data")]
    public IEnumerable<T> Data { get; set; } = Enumerable.Empty<T>();

    [JsonPropertyName("paging")]
    public Paging Paging { get; set; } = new();
    
    #endregion
    
    public static Paginated<T> FromJson(string json)
    {
        return JsonSerializer.Deserialize<Paginated<T>>(json, Options) ?? throw new JsonException("Failed to deserialize JSON response.");
    }
}

public class Paging
{
    [JsonPropertyName("next")]
    public string? Next { get; set; }
    
    [JsonPropertyName("previous")]
    public string? Previous { get; set; }
}