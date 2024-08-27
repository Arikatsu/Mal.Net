using System.Text.Json.Serialization;

namespace Mal.Net.Schemas.Anime;

public class AnimeList
{
    [JsonPropertyName("node")]
    public AnimeNode? Node { get; set; }
}