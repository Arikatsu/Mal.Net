using Mal.Net.Schemas.Auth;
using Mal.Net.Utils;
using Mal.Net.Exceptions;
using Mal.Net.Services;
using System.Text.Json;

namespace Mal.Net;

/// <summary>
/// Represents a user that has been authenticated with MyAnimeList.
/// </summary>
public class MalUser 
{
    #region Public Properties
    
    public string? AccessToken { get; }
    public string? RefreshToken { get; }
    public string? TokenType { get; }
    public DateTime AccessTokenExpiresAt { get; }
    
    #endregion
    
    #region Private Properties
    
    private readonly string _clientId;
    private readonly string? _clientSecret;
    
    #endregion
    
    /// <summary>
    /// Initializes a new instance of the <see cref="MalUser"/> class.
    /// </summary>
    /// <param name="response">The OAuth response.</param>
    /// <param name="clientId">The client ID.</param>
    /// <param name="clientSecret">The client secret.</param>
    public MalUser(OAuthResponse response, string clientId, string? clientSecret = null)
    {
        AccessToken = response.AccessToken;
        RefreshToken = response.RefreshToken;
        TokenType = response.TokenType;
        AccessTokenExpiresAt = DateTime.UtcNow.AddSeconds(response.ExpiresIn);
        
        _clientId = clientId;
        _clientSecret = clientSecret;
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="MalUser"/> class.
    /// </summary>
    /// <param name="accessToken">The access token.</param>
    /// <param name="refreshToken">The refresh token.</param>
    /// <param name="tokenType">The token type.</param>
    /// <param name="expiresIn">The number of seconds until the access token expires.</param>
    /// <param name="clientId">The client ID.</param>
    /// <param name="clientSecret">The client secret.</param>
    public MalUser(string accessToken, string refreshToken, string tokenType, int expiresIn, string clientId, string? clientSecret = null)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
        TokenType = tokenType;
        AccessTokenExpiresAt = DateTime.UtcNow.AddSeconds(expiresIn);
        
        _clientId = clientId;
        _clientSecret = clientSecret;
    }
    
    /// <summary>
    /// Determines whether the access token has expired.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if the access token has expired; otherwise, <see langword="false"/>.
    /// </returns>
    public bool IsAccessTokenExpired()
    {
        return DateTime.UtcNow >= AccessTokenExpiresAt;
    }
    
    /// <summary>
    /// Refreshes the access token.
    /// </summary>
    /// <returns>The refreshed user.</returns>
    /// <exception cref="MalHttpException">Thrown when the request to refresh the access token fails.</exception>
    /// <exception cref="JsonException">Thrown when the response from the server is not valid JSON.</exception>
    public async Task<MalUser> RefreshAccessToken()
    {
        var url = new ApiUrl("oauth2/token", forAuth: true);
        
        var keyValuePairs = new List<KeyValuePair<string?, string?>>
        {
            new("grant_type", "refresh_token"),
            new("refresh_token", RefreshToken),
            new("client_id", _clientId)
        };
        
        if (!string.IsNullOrEmpty(_clientSecret))
        {
            keyValuePairs.Add(new KeyValuePair<string?, string?>("client_secret", _clientSecret));
        }
        
        var content = new FormUrlEncodedContent(keyValuePairs);
        var response = await MalHttpClient.PostAsync(url.GetUrlWithoutParams(), content, "Failed to refresh access token");
        var data = OAuthResponse.FromJson(response);
        
        return new MalUser(data, _clientId, _clientSecret);
    }
}