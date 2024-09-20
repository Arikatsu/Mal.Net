using System.Text.Json.Serialization;

namespace Mal.Net.Models.Forum;

public class ForumTopic
{
    #region Json Properties
    
    [JsonPropertyName("id")]
    public int? Id { get; set; }
    
    [JsonPropertyName("title")]
    public string? Title { get; set; }
    
    [JsonPropertyName("created_at")]
    public string? CreatedAt { get; set; }
    
    [JsonPropertyName("created_by")]
    public CreatedBy? CreatedBy { get; set; }
    
    [JsonPropertyName("number_of_posts")]
    public int? NumberOfPosts { get; set; }
    
    [JsonPropertyName("last_post_created_at")]
    public string? LastPostCreatedAt { get; set; }
    
    [JsonPropertyName("last_post_created_by")]
    public CreatedBy? LastPostCreatedBy { get; set; }
    
    [JsonPropertyName("is_locked")]
    public bool? IsLocked { get; set; }
    
    #endregion
}