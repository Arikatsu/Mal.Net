using Mal.Net.Core;
using Mal.Net.Models;
using Mal.Net.Models.Auth;
using Mal.Net.Models.Forum;
using Mal.Net.Models.Manga;
using Mal.Net.Models.Anime;
using Mal.Net.Utils;

namespace Mal.Net;

public class MalUser : MalApiBase, IMalUser
{
    #region Public Properties
    
    public string? AccessToken { get; private set; }
    public string? RefreshToken { get; private set; }
    public string? TokenType { get; private set; }
    public DateTime AccessTokenExpiresAt { get; private set; }
    
    #endregion
    
    #region Private Properties
    
    private readonly string _clientId;
    private readonly string? _clientSecret;
    private readonly KeyValuePair<string, string> _header;
    
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
        _header = new KeyValuePair<string, string>("Authorization", $"{TokenType} {AccessToken}");
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
        _header = new KeyValuePair<string, string>("Authorization", $"{TokenType} {AccessToken}");
    }
    
    #region Authentication API Calls
    
    
    public bool IsAccessTokenExpired()
    {
        return DateTime.UtcNow >= AccessTokenExpiresAt;
    }
    
    public async Task<IMalUser> RefreshAccessTokenAsync(bool force = false, CancellationToken cancellationToken = default)
    {
        if (!force && !IsAccessTokenExpired())
        {
            return this;
        }
        
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
        var response = await MalHttpClient.PostAsync(url.GetUrlWithoutParams(), content, "Failed to refresh access token", cancellationToken);
        var data = OAuthResponse.FromJson(response);
        
        AccessToken = data.AccessToken;
        RefreshToken = data.RefreshToken;
        TokenType = data.TokenType;
        AccessTokenExpiresAt = DateTime.UtcNow.AddSeconds(data.ExpiresIn);

        return this;
    }
    
    
    #endregion
    
    #region Anime API Calls


    public async Task<Paginated<AnimeList>> GetAnimeListAsync(
        string? query = null, 
        MalRequestOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        return await GetAnimeListCoreAsync(_header, query, options, cancellationToken);
    }
    
    public async Task<AnimeNode> GetAnimeDetailsAsync(int animeId, IEnumerable<string>? fields = null, CancellationToken cancellationToken = default)
    {
        return await GetAnimeDetailsCoreAsync(_header, animeId, fields, cancellationToken);
    }
    
    public async Task<Paginated<RankedAnimeList>> GetAnimeRankingAsync(
        string rankingType, 
        MalRequestOptions? options = null, 
        CancellationToken cancellationToken = default)
    {
        return await GetAnimeRankingCoreAsync(_header, rankingType, options, cancellationToken);
    }
    
    public async Task<Paginated<AnimeList>> GetAnimeSeasonAsync(
        int year, 
        string season, 
        string? sort = null, 
        MalRequestOptions? options = null, 
        CancellationToken cancellationToken = default)
    {
        return await GetAnimeSeasonCoreAsync(_header, year, season, sort, options, cancellationToken);
    }
    
    public async Task<Paginated<AnimeList>> GetSuggestedAnimeAsync(
        MalRequestOptions? options = null, 
        CancellationToken cancellationToken = default)
    {
        return await GetSuggestedAnimeCoreAsync(_header, options, cancellationToken);
    }
    
    
    #endregion
    
    #region Forum API Calls
    
    
    public async Task<Forums> GetForumBoardsAsync(CancellationToken cancellationToken = default)
    {
        return await GetForumBoardsCoreAsync(_header, cancellationToken);
    }

    public async Task<Paginated<ForumTopicDetail>> GetForumTopicsDetailAsync(
        int topicId,
        MalRequestOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        return await GetForumTopicsDetailCoreAsync(_header, topicId, options, cancellationToken);
    }
    
    public async Task<Paginated<ForumTopic>> GetForumTopicsAsync(
        ForumTopicOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        return await GetForumTopicsCoreAsync(_header, options, cancellationToken);
    }
    
    
    #endregion
    
    #region Manga API Calls
    
    public async Task<Paginated<MangaList>> GetMangaListAsync(
        string? query = null, 
        MalRequestOptions? options = null, 
        CancellationToken cancellationToken = default)
    {
        return await GetMangaListCoreAsync(_header, query, options, cancellationToken);
    }
    
    public async Task<MangaNode> GetMangaDetailsAsync(
        int mangaId, 
        IEnumerable<string>? fields = null, 
        CancellationToken cancellationToken = default)
    {
        return await GetMangaDetailsCoreAsync(_header, mangaId, fields, cancellationToken);
    }
    
    public async Task<Paginated<RankedMangaList>> GetMangaRankingAsync(
        string rankingType, 
        MalRequestOptions? options = null, 
        CancellationToken cancellationToken = default)
    {
        return await GetMangaRankingCoreAsync(_header, rankingType, options, cancellationToken);
    }
    
    
    #endregion
}