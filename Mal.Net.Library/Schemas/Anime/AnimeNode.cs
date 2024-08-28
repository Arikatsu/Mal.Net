using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mal.Net.Schemas.Anime;

public class AnimeNode
{
    #region Json Properties
    
    [JsonPropertyName(AnimeFields.Id)]
    public int? Id { get; set; }
    
    [JsonPropertyName(AnimeFields.Title)]
    public string? Title { get; set; }
    
    [JsonPropertyName(AnimeFields.MainPicture)]
    public MainPicture? MainPicture { get; set; }
    
    [JsonPropertyName(AnimeFields.AlternativeTitles)]
    public AlternativeTitles? AlternativeTitles { get; set; }
    
    [JsonPropertyName(AnimeFields.StartDate)]
    public string? StartDate { get; set; }
    
    [JsonPropertyName(AnimeFields.EndDate)]
    public string? EndDate { get; set; }
    
    [JsonPropertyName(AnimeFields.Synopsis)]
    public string? Synopsis { get; set; }
    
    [JsonPropertyName(AnimeFields.Mean)]
    public float? Mean { get; set; }
    
    [JsonPropertyName(AnimeFields.Rank)]
    public int? Rank { get; set; }
    
    [JsonPropertyName(AnimeFields.Popularity)]
    public int? Popularity { get; set; }
    
    [JsonPropertyName(AnimeFields.NumListUsers)]
    public int? NumListUsers { get; set; }
    
    [JsonPropertyName(AnimeFields.NumScoringUsers)]
    public int? NumScoringUsers { get; set; }
    
    [JsonPropertyName(AnimeFields.Nsfw)]
    public string? Nsfw { get; set; }
    
    [JsonPropertyName(AnimeFields.Genres)]   
    public IEnumerable<Genre>? Genres { get; set; }
    
    [JsonPropertyName(AnimeFields.CreatedAt)]
    public string? CreatedAt { get; set; }
    
    [JsonPropertyName(AnimeFields.UpdatedAt)]
    public string? UpdatedAt { get; set; }
    
    [JsonPropertyName(AnimeFields.MediaType)]
    public string? MediaType { get; set; }
    
    [JsonPropertyName(AnimeFields.Status)]
    public string? Status { get; set; }
    
    [JsonPropertyName(AnimeFields.MyListStatus)]
    public MyListStatus? MyListStatus { get; set; }
    
    [JsonPropertyName(AnimeFields.NumEpisodes)]
    public int? NumEpisodes { get; set; }
    
    [JsonPropertyName(AnimeFields.StartSeason)]
    public StartSeason? StartSeason { get; set; }
    
    [JsonPropertyName(AnimeFields.Broadcast)]
    public Broadcast? Broadcast { get; set; }
    
    [JsonPropertyName(AnimeFields.Source)]
    public string? Source { get; set; }
    
    [JsonPropertyName(AnimeFields.AverageEpisodeDuration)]
    public int? AverageEpisodeDuration { get; set; }
    
    [JsonPropertyName(AnimeFields.Rating)]
    public string? Rating { get; set; }
    
    [JsonPropertyName(AnimeFields.Studios)]
    public IEnumerable<Studio>? Studios { get; set; }
    
    #endregion
    
    public static AnimeNode FromJson(string json)
    {
        return JsonSerializer.Deserialize<AnimeNode>(json) ?? throw new JsonException("Failed to deserialize JSON response.");
    }
}