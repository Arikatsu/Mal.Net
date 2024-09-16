using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mal.Net.Schemas.Auth;

public class OAuthResponse
{
    #region Json Properties
    
    [JsonPropertyName("token_type")]
    public string? TokenType { get; set; }
    
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }
    
    [JsonPropertyName("access_token")]
    public string? AccessToken { get; set; }
    
    [JsonPropertyName("refresh_token")]
    public string? RefreshToken { get; set; }
    
    #endregion
    
    public static OAuthResponse FromJson(string json)
    {
        return JsonSerializer.Deserialize<OAuthResponse>(json) ?? new OAuthResponse();
    }
}