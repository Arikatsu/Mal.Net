using System.Text.Json.Serialization;

namespace Mal.Net.Models.Forum;

public class SubBoard
{
    [JsonPropertyName("id")]
    public int? Id { get; set; }
    
    [JsonPropertyName("title")]
    public string? Title { get; set; }
}

public class CreatedBy
{
    [JsonPropertyName("id")]
    public int? Id { get; set; }
    
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    
    [JsonPropertyName("forum_avatar")]
    public string? ForumAvatar { get; set; }
}

public class Option
{
    [JsonPropertyName("id")]
    public int? Id { get; set; }
    
    [JsonPropertyName("text")]
    public string? Text { get; set; }
    
    [JsonPropertyName("votes")]
    public int? Votes { get; set; }
}
