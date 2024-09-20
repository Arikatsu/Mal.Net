using System.Text.Json.Serialization;

namespace Mal.Net.Models.Forum;

public class ForumCategory
{
    #region Json Properties
    
    [JsonPropertyName("title")]
    public string? Title { get; set; }
    
    [JsonPropertyName("boards")]
    public IEnumerable<ForumBoards>? Boards { get; set; }
    
    #endregion
}