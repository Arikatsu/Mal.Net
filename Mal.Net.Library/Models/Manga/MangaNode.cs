using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mal.Net.Models.Manga;

public class MangaNode
{
    #region Json Properties
    
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("title")]
    public string? Title { get; set; }
    
    [JsonPropertyName("main_picture")]
    public MainPicture? MainPicture { get; set; }
    
    [JsonPropertyName("alternative_titles")]
    public AlternativeTitles? AlternativeTitles { get; set; }
    
    [JsonPropertyName("start_date")]
    public string? StartDate { get; set; }
    
    [JsonPropertyName("end_date")]
    public string? EndDate { get; set; }
    
    [JsonPropertyName("synopsis")]
    public string? Synopsis { get; set; }
    
    [JsonPropertyName("mean")]
    public float? Mean { get; set; }
    
    [JsonPropertyName("rank")]
    public int? Rank { get; set; }
    
    [JsonPropertyName("popularity")]
    public int? Popularity { get; set; }
    
    [JsonPropertyName("num_list_users")]
    public int? NumListUsers { get; set; }
    
    [JsonPropertyName("num_scoring_users")]
    public int? NumScoringUsers { get; set; }
    
    [JsonPropertyName("nsfw")]
    public string? Nsfw { get; set; }
    
    [JsonPropertyName("genres")]
    public IEnumerable<Genre>? Genres { get; set; }
    
    [JsonPropertyName("created_at")]
    public string? CreatedAt { get; set; }
    
    [JsonPropertyName("updated_at")]
    public string? UpdatedAt { get; set; }
    
    [JsonPropertyName("media_type")]
    public string? MediaType { get; set; }
    
    [JsonPropertyName("status")]
    public string? Status { get; set; }
    
    [JsonPropertyName("my_list_status")]
    public MyListStatus? MyListStatus { get; set; }
    
    [JsonPropertyName("num_volumes")]
    public int? NumVolumes { get; set; }
    
    [JsonPropertyName("num_chapters")]
    public int? NumChapters { get; set; }
    
    [JsonPropertyName("authors")]
    public IEnumerable<Author>? Authors { get; set; }

    #endregion
    
    public static MangaNode FromJson(string json)
    {
        return JsonSerializer.Deserialize<MangaNode>(json) ?? throw new JsonException("Failed to deserialize JSON response.");
    }
}