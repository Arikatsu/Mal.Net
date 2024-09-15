using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mal.Net.Schemas.Forum;

public class ForumBoards
{
    # region Json Properties
    
    [JsonPropertyName("id")]
    public int? Id { get; set; }
    
    [JsonPropertyName("title")]
    public string? Title { get; set; }
    
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    
    [JsonPropertyName("subboards")]
    public IEnumerable<SubBoard>? SubBoards { get; set; }
    
    #endregion
}