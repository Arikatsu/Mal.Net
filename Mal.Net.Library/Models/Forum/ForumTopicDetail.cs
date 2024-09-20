using System.Text.Json.Serialization;

namespace Mal.Net.Models.Forum;

public class ForumTopicDetail
{
    #region Json Properties
    
    [JsonPropertyName("title")]
    public string? Title { get; set; }
    
    [JsonPropertyName("posts")]
    public IEnumerable<ForumPost>? Posts { get; set; }
    
    [JsonPropertyName("poll")]
    public IEnumerable<ForumPoll>? Poll { get; set; }
    
    #endregion
}