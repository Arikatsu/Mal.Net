using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mal.Net.Schemas.Anime;

public class RankedAnimeList
{
    [JsonPropertyName("node")]
    public AnimeNode? Node { get; set; }
    
    [JsonPropertyName("ranking")]
    public Ranking? Ranking { get; set; }
}