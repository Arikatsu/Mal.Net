using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mal.Net.Schemas.Forum;

public class ForumPoll
{
    #region Json Properties
    
    [JsonPropertyName("id")]
    public int? Id { get; set; }
    
    [JsonPropertyName("question")]
    public string? Question { get; set; }
    
    [JsonPropertyName("close")]
    public bool? Close { get; set; }
    
    [JsonPropertyName("options")]
    public IEnumerable<Option>? Options { get; set; }
    
    #endregion
}