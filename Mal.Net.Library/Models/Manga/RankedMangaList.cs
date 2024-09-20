using System.Text.Json.Serialization;

namespace Mal.Net.Models.Manga;

public class RankedMangaList
{
    #region Json Properties

    [JsonPropertyName("node")]
    public MangaNode? Node { get; set; }
    
    [JsonPropertyName("ranking")]
    public Ranking? Ranking { get; set; }
    
    #endregion
}