using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mal.Net.Schemas.Forum;

public class ForumPost
{
    #region Json Properties
    
    [JsonPropertyName("id")]
    public int? Id { get; set; }
    
    [JsonPropertyName("number")]
    public int? Number { get; set; }
    
    [JsonPropertyName("created_at")]
    public string? CreatedAt { get; set; }
    
    [JsonPropertyName("created_by")]
    public CreatedBy? CreatedBy { get; set; }
    
    [JsonPropertyName("body")]
    public string? Body { get; set; }
    
    [JsonPropertyName("signature")]
    public string? Signature { get; set; }
    
    #endregion
}