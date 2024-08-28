using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mal.Net.Schemas;

public class Paginated<T>
{
    # region Json Properties
    
    [JsonPropertyName("data")]
    public IEnumerable<T> Data { get; set; } = Enumerable.Empty<T>();
    
    [JsonPropertyName("paging")]
    public Paging Paging { get; set; } = new Paging();
    
    #endregion
    
    public static Paginated<T> FromJson(string json)
    {
        return JsonSerializer.Deserialize<Paginated<T>>(json) ?? throw new JsonException("Failed to deserialize JSON response.");
    }
}

public class Paging
{
    [JsonPropertyName("next")]
    public string? Next { get; set; }
    
    [JsonPropertyName("previous")]
    public string? Previous { get; set; }
}