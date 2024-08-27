using System.Text.Json.Serialization;

namespace Mal.Net.Schemas;

public class Paginated<T>
{
    [JsonPropertyName("data")]
    public IEnumerable<T> Data { get; set; } = Enumerable.Empty<T>();
    
    [JsonPropertyName("paging")]
    public Paging Paging { get; set; } = new Paging();
}

public class Paging
{
    [JsonPropertyName("next")]
    public string? Next { get; set; }
    
    [JsonPropertyName("previous")]
    public string? Previous { get; set; }
}