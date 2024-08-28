using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mal.Net.Schemas.Anime;

public class AnimeList
{
    # region Json Properties
    
    [JsonPropertyName("node")]
    public AnimeNode? Node { get; set; }
    
    #endregion
    
    public static AnimeList FromJson(string json)
    {
        return JsonSerializer.Deserialize<AnimeList>(json) ?? throw new JsonException("Failed to deserialize JSON response.");
    }
}