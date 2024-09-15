using System.Text.Json.Serialization;

namespace Mal.Net.Schemas.Manga;

public class MainPicture
{
    [JsonPropertyName("medium")]
    public string? Medium { get; set; }
    
    [JsonPropertyName("large")]
    public string? Large { get; set; }
}

public class AlternativeTitles
{
    [JsonPropertyName("synonyms")]
    public IEnumerable<string>? Synonyms { get; set; }
    
    [JsonPropertyName("en")]
    public string? En { get; set; }
    
    [JsonPropertyName("ja")]
    public string? Ja { get; set; }
}

public class Genre
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

public class MyListStatus
{
    [JsonPropertyName(MyListStatusFields.Status)]
    public string? Status { get; set; }
    
    [JsonPropertyName(MyListStatusFields.Score)]
    public int? Score { get; set; }
    
    [JsonPropertyName(MyListStatusFields.NumVolumesRead)]
    public int? NumVolumesRead { get; set; }
    
    [JsonPropertyName(MyListStatusFields.NumChaptersRead)]
    public int? NumChaptersRead { get; set; }
    
    [JsonPropertyName(MyListStatusFields.IsRereading)]
    public bool? IsRereading { get; set; }
    
    [JsonPropertyName(MyListStatusFields.StartDate)]
    public string? StartDate { get; set; }
    
    [JsonPropertyName(MyListStatusFields.FinishDate)]
    public string? FinishDate { get; set; }
    
    [JsonPropertyName(MyListStatusFields.Priority)]
    public int? Priority { get; set; }
    
    [JsonPropertyName(MyListStatusFields.NumTimesReread)]
    public int? NumTimesReread { get; set; }
    
    [JsonPropertyName(MyListStatusFields.RereadValue)]
    public int? RereadValue { get; set; }
    
    [JsonPropertyName(MyListStatusFields.Tags)]
    public IEnumerable<string>? Tags { get; set; }
    
    [JsonPropertyName(MyListStatusFields.Comments)]
    public string? Comments { get; set; }
    
    [JsonPropertyName(MyListStatusFields.UpdatedAt)]
    public string? UpdatedAt { get; set; }
}

public class Author
{
    [JsonPropertyName("node")]
    public AuthorNode? Node { get; set; }
    
    [JsonPropertyName("role")]
    public string? Role { get; set; }
}

public class AuthorNode
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("first_name")]
    public string? FirstName { get; set; }
    
    [JsonPropertyName("last_name")]
    public string? LastName { get; set; }
}

public class Ranking
{
    [JsonPropertyName("rank")]
    public int? Rank { get; set; }
    
    [JsonPropertyName("previous_rank")]
    public int? PreviousRank { get; set; }
}