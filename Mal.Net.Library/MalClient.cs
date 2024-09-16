using System.Text.Json;
using Mal.Net.Services;
using Mal.Net.Utils;
using Mal.Net.Exceptions;
using Mal.Net.Schemas;
using Mal.Net.Schemas.Anime;
using Mal.Net.Schemas.Forum;
using Mal.Net.Schemas.Manga;
using Mal.Net.Schemas.Auth;

namespace Mal.Net;

/// <summary>
/// Represents the main entry point for interacting with the MyAnimeList API.
/// </summary>
public class MalClient : IDisposable, IAnimeService, IForumService, IMangaService
{
    
    private readonly string _clientId;
    private readonly string? _clientSecret;

    /// <summary>
    /// Initializes a new instance of the <see cref="MalClient"/> class using only the client ID.
    /// This constructor is intended for Android, iOS, and Other app types that cannot safely store the client secret.
    /// </summary>
    /// <param name="clientId">The client ID provided by MyAnimeList.</param>
    public MalClient(string clientId)
    {
        _clientId = clientId;
        
        MalHttpClient.SetClientId(clientId);
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
        
        MalHttpClient.SetClientId(clientId);
    }

    /// <summary>
    /// Releases all resources used by the current instance of the <see cref="MalClient"/> class.
    /// </summary>
    public void Dispose()
    {
        MalHttpClient.Dispose();

        GC.SuppressFinalize(this);
    }
    
    
    #region Authentication API Calls
    
    
    /// <summary>
    /// Generates the URL for the OAuth2 authorization endpoint.
    /// </summary>
    /// <param name="state">The state parameter to include in the URL.</param>
    /// <param name="codeVerifier">The code verifier parameter to include in the URL.</param>
    /// <param name="redirectUri">The redirect URI to include in the URL. Leave null if only one redirect URI was specified while creating the MAL API application.</param>
    /// <returns>The URL for the OAuth2 authorization endpoint.</returns>
    public string GenerateAuthUrl(out string state, out string? codeVerifier, string? redirectUri = null)
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
    
    /// <summary>
    /// Authenticates a user using the provided authorization code.
    /// </summary>
    /// <param name="code">The authorization code to use for authentication.</param>
    /// <param name="codeVerifier">The code verifier used to generate the authorization code.</param>
    /// <param name="redirectUri">The redirect URI used to generate the authorization code. Leave null if only one redirect URI was specified while creating the MAL API application.</param>
    /// <returns>A <see cref="MalUser"/> object representing the authenticated user.</returns>
    /// <exception cref="MalHttpException">Thrown when the request to the MyAnimeList API fails.</exception>
    /// <exception cref="JsonException">Thrown when the response from the MyAnimeList API cannot be deserialized.</exception>
    public async Task<MalUser> AuthenticateUser(string code, string codeVerifier, string? redirectUri = null)
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
        var response = await MalHttpClient.PostAsync(url.GetUrlWithoutParams(), content);
        var data = OAuthResponse.FromJson(response);
        
        return new MalUser(data, _clientId, _clientSecret);
    }
    
    
    #endregion


    #region Anime API Calls

    
    /// <inheritdoc/>
    /// <param name="query">The search query to filter anime.</param>
    /// <param name="limit">The maximum number of results to return. Default is 100.</param>
    /// <param name="offset">The number of results to skip before starting to return results. Default is 0.</param>
    /// <param name="fields">Additional fields to include in the JSON response. Default is null.</param>
    /// <param name="includeNsfw">Whether to include NSFW content in the results. Default is false.</param>
    public async Task<Paginated<AnimeList>> GetAnimeListAsync(string? query = null, int limit = 100, int offset = 0, IEnumerable<string>? fields = null, bool includeNsfw = false)
    {
        var url = new ApiUrl("anime", new { limit, offset, nsfw = includeNsfw })
            .AddParamIf("q", query)
            .AddParamIf("fields", StringHelper.ToCommaSeparatedString(fields ?? Enumerable.Empty<string>()));
        
        var error = "Failed to retrieve anime list" + (query != null ? $" for query '{query}'" : string.Empty);
        var response = await MalHttpClient.GetAsync(url.GetUrl(), error);
        var data = Paginated<AnimeList>.FromJson(response);
        
        return data;
    }
    
    /// <inheritdoc/>
    /// <param name="animeId">The ID of the anime to retrieve details for.</param>
    /// <param name="fields">Additional fields to include in the JSON response. Default is null.</param>
    public async Task<AnimeNode> GetAnimeDetailsAsync(int animeId, IEnumerable<string>? fields = null)
    {
        var url = new ApiUrl($"anime/{animeId}")
            .AddParamIf("fields", StringHelper.ToCommaSeparatedString(fields ?? Enumerable.Empty<string>()));
        
        var error = $"Failed to retrieve anime details for ID '{animeId}'";
        var response = await MalHttpClient.GetAsync(url.GetUrl(), error);
        var data = AnimeNode.FromJson(response);
        
        return data;
    }
    
    /// <inheritdoc/>
    /// <param name="rankingType">The type of ranking to retrieve. Valid types include "all", "airing", "upcoming", "tv", "ova", "movie", "special", "bypopularity", "favorite".</param>
    /// <param name="limit">The maximum number of results to return. Default is 100.</param>
    /// <param name="offset">The number of results to skip before starting to return results. Default is 0.</param>
    /// <param name="fields">Additional fields to include in the JSON response. Default is null.</param>
    public async Task<Paginated<RankedAnimeList>> GetAnimeRankingAsync(string rankingType, int limit = 100, int offset = 0, IEnumerable<string>? fields = null)
    {
        var url = new ApiUrl("anime/ranking", new { ranking_type = rankingType, limit, offset })
            .AddParamIf("fields", StringHelper.ToCommaSeparatedString(fields ?? Enumerable.Empty<string>()));
        
        var error = $"Failed to retrieve anime ranking for type '{rankingType}'";
        var response = await MalHttpClient.GetAsync(url.GetUrl(), error);
        var data = Paginated<RankedAnimeList>.FromJson(response);
        
        return data;
    }

    /// <inheritdoc/>
    /// <param name="year">The year to retrieve the season for.</param>
    /// <param name="season">The season to retrieve. Valid seasons include "winter", "spring", "summer", "fall".</param>
    /// <param name="sort">The sort order to use. Valid options include "anime_score", "anime_num_list_users".</param>
    /// <param name="limit">The maximum number of results to return. Default is 100.</param>
    /// <param name="offset">The number of results to skip before starting to return results. Default is 0.</param>
    /// <param name="fields">Additional fields to include in the JSON response. Default is null.</param>
    /// <param name="includeNsfw">Whether to include NSFW content in the results. Default is false.</param>
    public async Task<Paginated<AnimeList>> GetAnimeSeasonAsync(int year, string season, string? sort = null, int limit = 100, int offset = 0, IEnumerable<string>? fields = null, bool includeNsfw = false)
    {
        var url = new ApiUrl($"anime/season/{year}/{season}", new { limit, offset, nsfw = includeNsfw })
            .AddParamIf("fields", StringHelper.ToCommaSeparatedString(fields ?? Enumerable.Empty<string>()))
            .AddParamIf("sort", sort);
        
        var error = $"Failed to retrieve anime season for {season} {year}";
        var response = await MalHttpClient.GetAsync(url.GetUrl(), error);
        var data = Paginated<AnimeList>.FromJson(response);
        
        return data;
    }


    #endregion
    
    
    #region Forum API Calls
    
    
    /// <inheritdoc/>
    public async Task<Forums> GetForumBoardsAsync()
    {
        var url = new ApiUrl("forum/boards");
        
        const string error = "Failed to retrieve forum boards";
        var response = await MalHttpClient.GetAsync(url.GetUrl(), error);
        var data = Forums.FromJson(response);
        
        return data;
    }
    
    /// <inheritdoc/>
    /// <param name="topicId">The ID of the topic to retrieve details for.</param>
    /// <param name="limit">The maximum number of results to return. Default is 100.</param>
    /// <param name="offset">The number of results to skip before starting to return results. Default is 0.</param>
    public async Task<Paginated<ForumTopicDetail>> GetForumTopicsDetailAsync(int topicId, int limit = 100, int offset = 0)
    {
        var url = new ApiUrl($"forum/topic/{topicId}", new { limit, offset });
        
        var error = $"Failed to retrieve forum topics for ID '{topicId}'";
        var response = await MalHttpClient.GetAsync(url.GetUrl(), error);
        var data = Paginated<ForumTopicDetail>.FromJson(response);
        
        return data;
    }
    
    /// <inheritdoc/>
    /// <param name="boardId">The ID of the board to retrieve topics for. Default is null.</param>
    /// <param name="subBoardId">The ID of the sub-board to retrieve topics for. Default is null.</param>
    /// <param name="query">The search query to filter topics. Default is null.</param>
    /// <param name="topicUserName">The username of the topic creator. Default is null.</param>
    /// <param name="userName">The username of the topic poster. Default is null.</param>
    /// <param name="limit">The maximum number of results to return. Default is 100.</param>
    /// <param name="offset">The number of results to skip before starting to return results. Default is 0.</param>
    public async Task<Paginated<ForumTopic>> GetForumTopicsAsync(int? boardId = null, int? subBoardId = null, string? query = null, string? topicUserName = null, string? userName = null, int limit = 100, int offset = 0)
    {
        var url = new ApiUrl("forum/topics", new { limit, offset })
            .AddParamIf("board_id", boardId)
            .AddParamIf("subboard_id", subBoardId)
            .AddParamIf("q", query)
            .AddParamIf("topic_user_name", topicUserName)
            .AddParamIf("user_name", userName);
        
        var error = "Failed to retrieve forum topics for"
            + (boardId != null ? $" board ID '{boardId}'." : string.Empty)
            + (subBoardId != null ? $" sub-board ID '{subBoardId}'." : string.Empty)
            + (query != null ? $" query '{query}'." : string.Empty)
            + (topicUserName != null ? $" topic user name '{topicUserName}'." : string.Empty)
            + (userName != null ? $" user name '{userName}'." : string.Empty);
        
        var response = await MalHttpClient.GetAsync(url.GetUrl(), error);
        var data = Paginated<ForumTopic>.FromJson(response);
        
        return data;
    }
    
    
    #endregion
    
    
    #region Manga API Calls
    
    
    /// <inheritdoc/>
    /// <param name="query">The search query to filter manga.</param>
    /// <param name="limit">The maximum number of results to return. Default is 100.</param>
    /// <param name="offset">The number of results to skip before starting to return results. Default is 0.</param>
    /// <param name="fields">Additional fields to include in the JSON response. Default is null.</param>
    /// <param name="includeNsfw">Whether to include NSFW content in the results. Default is false.</param>
    public async Task<Paginated<MangaList>> GetMangaListAsync(string? query = null, int limit = 100, int offset = 0, IEnumerable<string>? fields = null, bool includeNsfw = false)
    {
        var url = new ApiUrl("manga", new { limit, offset, nsfw = includeNsfw })
            .AddParamIf("q", query)
            .AddParamIf("fields", StringHelper.ToCommaSeparatedString(fields ?? Enumerable.Empty<string>()));
        
        var error = "Failed to retrieve manga list" + (query != null ? $" for query '{query}'" : string.Empty);
        var response = await MalHttpClient.GetAsync(url.GetUrl(), error);
        var data = Paginated<MangaList>.FromJson(response);
        
        return data;
    }
    
    /// <inheritdoc/>
    /// <param name="mangaId">The ID of the manga to retrieve details for.</param>
    /// <param name="fields">Additional fields to include in the JSON response. Default is null.</param>
    public async Task<MangaNode> GetMangaDetailsAsync(int mangaId, IEnumerable<string>? fields = null)
    {
        var url = new ApiUrl($"manga/{mangaId}")
            .AddParamIf("fields", StringHelper.ToCommaSeparatedString(fields ?? Enumerable.Empty<string>()));
        
        var error = $"Failed to retrieve manga details for ID '{mangaId}'";
        var response = await MalHttpClient.GetAsync(url.GetUrl(), error);
        var data = MangaNode.FromJson(response);
        
        return data;
    }
    
    /// <inheritdoc/>
    /// <param name="rankingType">The type of ranking to retrieve. Valid types include "all", "manga", "novels", "oneshots", "doujin", "manhwa", "manhua", "bypopularity", "favorite".</param>
    /// <param name="limit">The maximum number of results to return. Default is 100.</param>
    /// <param name="offset">The number of results to skip before starting to return results. Default is 0.</param>
    /// <param name="fields">Additional fields to include in the JSON response. Default is null.</param>
    public async Task<Paginated<RankedMangaList>> GetMangaRankingAsync(string rankingType, int limit = 100, int offset = 0, IEnumerable<string>? fields = null)
    {
        var url = new ApiUrl("manga/ranking", new { ranking_type = rankingType, limit, offset })
            .AddParamIf("fields", StringHelper.ToCommaSeparatedString(fields ?? Enumerable.Empty<string>()));
        
        var error = $"Failed to retrieve manga ranking for type '{rankingType}'";
        var response = await MalHttpClient.GetAsync(url.GetUrl(), error);
        var data = Paginated<RankedMangaList>.FromJson(response);
        
        return data;
    }
    
    
    #endregion
}