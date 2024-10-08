﻿using System.Text.Json.Serialization;

namespace Mal.Net.Models.Anime;

public class RankedAnimeList
{
    #region Json Properties
    
    [JsonPropertyName("node")]
    public AnimeNode? Node { get; set; }
    
    [JsonPropertyName("ranking")]
    public Ranking? Ranking { get; set; }
    
    #endregion
}