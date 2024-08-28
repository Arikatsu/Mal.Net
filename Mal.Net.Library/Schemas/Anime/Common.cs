using System.Text.Json.Serialization;

namespace Mal.Net.Schemas.Anime;

public class MainPicture
{
    [JsonPropertyName("large")]
    public string Large { get; set; } = string.Empty;
    
    [JsonPropertyName("medium")]
    public string Medium { get; set; } = string.Empty;
}

public class AlternativeTitles
{
    [JsonPropertyName("synonyms")]
    public IEnumerable<string> Synonyms { get; set; } = Enumerable.Empty<string>();
    
    [JsonPropertyName("en")]
    public string? English { get; set; }
    
    [JsonPropertyName("ja")]
    public string? Japanese { get; set; }
}

public class Genre
{
    [JsonPropertyName("id")]
    public int? Id { get; set; }
    
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

public class MyListStatus
{
    [JsonPropertyName(MyListStatusFields.Status)]
    public string? Status { get; set; }
    
    [JsonPropertyName(MyListStatusFields.Score)]
    public int? Score { get; set; }
    
    [JsonPropertyName(MyListStatusFields.NumEpisodesWatched)]
    public int? NumEpisodesWatched { get; set; }
    
    [JsonPropertyName(MyListStatusFields.IsReWatching)]
    public bool? IsReWatching { get; set; }
    
    [JsonPropertyName(MyListStatusFields.StartDate)]
    public string? StartDate { get; set; }
    
    [JsonPropertyName(MyListStatusFields.FinishDate)]
    public string? FinishDate { get; set; }
    
    [JsonPropertyName(MyListStatusFields.Priority)]
    public int? Priority { get; set; }
    
    [JsonPropertyName(MyListStatusFields.NumTimesReWatched)]
    public int? NumTimesReWatched { get; set; }
    
    [JsonPropertyName(MyListStatusFields.ReWatchValue)]
    public int? ReWatchValue { get; set; }
    
    [JsonPropertyName(MyListStatusFields.Tags)]
    public IEnumerable<string>? Tags { get; set; }
    
    [JsonPropertyName(MyListStatusFields.Comments)]
    public string? Comments { get; set; }
    
    [JsonPropertyName(MyListStatusFields.UpdatedAt)]
    public string? UpdatedAt { get; set; }
}

public class StartSeason
{
    [JsonPropertyName("year")]
    public int? Year { get; set; }
    
    [JsonPropertyName("season")]
    public string? Season { get; set; }
}

public class Broadcast
{
    [JsonPropertyName("day_of_the_week")]
    public string? DayOfTheWeek { get; set; }
    
    [JsonPropertyName("start_time")]
    public string? StartTime { get; set; }
}

public class Studio
{
    [JsonPropertyName("id")]
    public int? Id { get; set; }
    
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

public class Ranking
{
    [JsonPropertyName("rank")]
    public int? Rank { get; set; }
    
    [JsonPropertyName("previous_rank")]
    public int? PreviousRank { get; set; }
}