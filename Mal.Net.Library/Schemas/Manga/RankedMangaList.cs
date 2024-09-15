using System.Text.Json.Serialization;

namespace Mal.Net.Schemas.Manga;

public class RankedMangaList
{
    #region Json Properties

    [JsonPropertyName("node")]
    public MangaNode? Node { get; set; }
    
    [JsonPropertyName("ranking")]
    public Ranking? Ranking { get; set; }
    
    #endregion
}