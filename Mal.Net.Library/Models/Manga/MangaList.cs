using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mal.Net.Models.Manga;

public class MangaList
{
    #region Json Properties
    
    [JsonPropertyName("node")]
    public MangaNode? Node { get; set; }

    #endregion
    
    public static MangaList FromJson(string json)
    {
        return JsonSerializer.Deserialize<MangaList>(json) ?? throw new JsonException("Failed to deserialize JSON response.");
    }
}