using System.Text.Json;
using Mal.Net.Schemas;
using Mal.Net.Schemas.Auth;
using Mal.Net.Schemas.Anime;
using Mal.Net.Schemas.Forum;
using Mal.Net.Schemas.Manga;
using Mal.Net.Utils;
using Mal.Net.Exceptions;
using Mal.Net.Services.Base;

namespace Mal.Net;

/// <summary>
/// Represents a user that has been authenticated with MyAnimeList.
/// </summary>
public class MalUser : MalUserApiBase
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
        : base(response.AccessToken, response.TokenType)
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
        : base(accessToken, tokenType)
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
    
    
    #region Anime API Calls
    
    
    /// <inheritdoc />
    /// <param name="query">The search query to filter anime.</param>
    /// <param name="limit">The maximum number of results to return. Default is 100.</param>
    /// <param name="offset">The number of results to skip before starting to return results. Default is 0.</param>
    /// <param name="fields">Additional fields to include in the JSON response. Default is null.</param>
    /// <param name="includeNsfw">Whether to include NSFW content in the results. Default is false.</param>
    public new async Task<Paginated<AnimeList>> GetAnimeListAsync(string? query = null, int limit = 100, int offset = 0, IEnumerable<string>? fields = null, bool includeNsfw = false)
    {
        return await base.GetAnimeListAsync(query, limit, offset, fields, includeNsfw);
    }
    
    /// <inheritdoc/>
    /// <param name="animeId">The ID of the anime to retrieve details for.</param>
    /// <param name="fields">Additional fields to include in the JSON response. Default is null.</param>
    public new async Task<AnimeNode> GetAnimeDetailsAsync(int animeId, IEnumerable<string>? fields = null)
    {
        return await base.GetAnimeDetailsAsync(animeId, fields);
    }
    
    /// <inheritdoc/>
    /// <param name="rankingType">The type of ranking to retrieve. Possible values are "all", "airing", "upcoming", "tv", "ova", "movie", "special", "bypopularity", "favorite".</param>
    /// <param name="limit">The maximum number of results to return. Default is 100.</param>
    /// <param name="offset">The number of results to skip before starting to return results. Default is 0.</param>
    /// <param name="fields">Additional fields to include in the JSON response. Default is null.</param>
    public new async Task<Paginated<RankedAnimeList>> GetAnimeRankingAsync(string rankingType, int limit = 100, int offset = 0, IEnumerable<string>? fields = null)
    {
        return await base.GetAnimeRankingAsync(rankingType, limit, offset, fields);
    }
    
    /// <inheritdoc/>
    /// <param name="year">The year to retrieve anime for.</param>
    /// <param name="season">The season to retrieve anime for. Possible values are "winter", "spring", "summer", "fall".</param>
    /// <param name="sort">The sort order of the results. Possible values are "anime_score", "anime_num_list_users", "anime_num_scoring_users", "anime_num_completed_users", "anime_num_episodes", "anime_start_date", "anime_end_date".</param>
    /// <param name="limit">The maximum number of results to return. Default is 100.</param>
    /// <param name="offset">The number of results to skip before starting to return results. Default is 0.</param>
    /// <param name="fields">Additional fields to include in the JSON response. Default is null.</param>
    /// <param name="includeNsfw">Whether to include NSFW content in the results. Default is false.</param>
    public new async Task<Paginated<AnimeList>> GetAnimeSeasonAsync(int year, string season, string sort, int limit = 100, int offset = 0, IEnumerable<string>? fields = null, bool includeNsfw = false)
    {
        return await base.GetAnimeSeasonAsync(year, season, sort, limit, offset, fields, includeNsfw);
    }
    
    /// <inheritdoc/>
    /// <param name="limit">The maximum number of results to return. Default is 100.</param>
    /// <param name="offset">The number of results to skip before starting to return results. Default is 0.</param>
    /// <param name="fields">Additional fields to include in the JSON response. Default is null.</param>
    /// <param name="includeNsfw">Whether to include NSFW content in the results. Default is false.</param>
    public new async Task<Paginated<AnimeList>> GetSuggestedAnimeAsync(int limit = 100, int offset = 0, IEnumerable<string>? fields = null, bool includeNsfw = false)
    {
        return await base.GetSuggestedAnimeAsync(limit, offset, fields, includeNsfw);
    }
    
    
    #endregion
}