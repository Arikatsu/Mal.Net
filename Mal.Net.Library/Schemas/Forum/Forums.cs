using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mal.Net.Schemas.Forum;

public class Forums
{
    #region Json Properties
    
    [JsonPropertyName("categories")]
    public IEnumerable<ForumCategory>? Categories { get; set; }
    
    #endregion
    
    public static Forums FromJson(string json)
    {
        return JsonSerializer.Deserialize<Forums>(json) ?? throw new JsonException("Failed to deserialize JSON response.");
    }
}