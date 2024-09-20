using System.Text.Json;
using Mal.Net.Utils;
using Mal.Net.Exceptions;
using Mal.Net.Core;
using Mal.Net.Models;
using Mal.Net.Models.Anime;
using Mal.Net.Models.Auth;
using Mal.Net.Models.Forum;
using Mal.Net.Models.Manga;

namespace Mal.Net;

public class MalClient : MalApiBase, IMalClient
{
    
    private readonly string _clientId;
    private readonly string? _clientSecret;
    private readonly KeyValuePair<string, string> _header;

    /// <summary>
    /// Initializes a new instance of the <see cref="MalClient"/> class using only the client ID.
    /// This constructor is intended for Android, iOS, and Other app types that cannot safely store the client secret.
    /// </summary>
    /// <param name="clientId">The client ID provided by MyAnimeList.</param>
    public MalClient(string clientId)
    {
        _clientId = clientId;
        _header = new KeyValuePair<string, string>("X-MAL-Client-ID", clientId);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MalClient"/> class using both the client ID and client secret.
    /// This constructor is intended for Web app types that can safely store the client secret.
    /// </summary>
    /// <param name="clientId">The client ID provided by MyAnimeList.</param>
    /// <param name="clientSecret">The client secret provided by MyAnimeList.</param>
    public MalClient(string clientId, string clientSecret)
    {
        _clientId = clientId;
        _clientSecret = clientSecret;
        _header = new KeyValuePair<string, string>("X-MAL-Client-ID", clientId);
    }
    
    
    #region Authentication API Calls
    
    public string GenerateAuthorizationUrl(out string state, out string? codeVerifier, string? redirectUri = null)
    {
        state = AuthHelper.GenerateState();
        codeVerifier = AuthHelper.GenerateCodeVerifier();
        
        var codeChallenge = codeVerifier;
        
        var url = new ApiUrl("oauth2/authorize", new
        {
            response_type = "code",
            client_id = _clientId,
            state, 
            code_challenge = codeChallenge, 
            code_challenge_method = "plain"
        }, forAuth: true)
            .AddParamIf("redirect_uri", redirectUri);
        
        return url.GetUrl();
    }
    
    public async Task<MalUser> AuthenticateUserAsync(string code, string codeVerifier, string? redirectUri = null, CancellationToken cancellationToken = default)
    {
        var url = new ApiUrl("oauth2/token", forAuth: true);
        
        var keyValuePairs = new List<KeyValuePair<string, string>>
        {
            new("client_id", _clientId),
            new("code", code),
            new("code_verifier", codeVerifier),
            new("grant_type", "authorization_code")
        };
        
        if (!string.IsNullOrEmpty(_clientSecret))
        {
            keyValuePairs.Add(new KeyValuePair<string, string>("client_secret", _clientSecret));
        }
        
        if (!string.IsNullOrEmpty(redirectUri))
        {
            keyValuePairs.Add(new KeyValuePair<string, string>("redirect_uri", redirectUri));
        }
        
        var content = new FormUrlEncodedContent(keyValuePairs);
        var response = await MalHttpClient.PostAsync(url.GetUrlWithoutParams(), content, "Failed to authenticate user", cancellationToken);
        var data = OAuthResponse.FromJson(response);
        
        return new MalUser(data, _clientId, _clientSecret);
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