using Mal.Net.Services;
using Mal.Net.Utils;
using Mal.Net.Schemas;
using Mal.Net.Schemas.Anime;
using Mal.Net.Schemas.Forum;

namespace Mal.Net;

/// <summary>
/// Represents the main entry point for interacting with the MyAnimeList API.
/// </summary>
public class MalClient : IDisposable, IAnimeService, IForumService
{
    private readonly MalHttpClient _httpClient;

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
        _httpClient = new MalHttpClient(clientId);
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
        _httpClient = new MalHttpClient(clientId);
    }

    /// <summary>
    /// Releases all resources used by the current instance of the <see cref="MalClient"/> class.
    /// </summary>
    public void Dispose()
    {
        _httpClient?.Dispose();

        GC.SuppressFinalize(this);
    }


    #region Anime API Calls

    
    /// <inheritdoc/>
    /// <param name="query">The search query to filter anime.</param>
    /// <param name="limit">The maximum number of results to return. Default is 100.</param>
    /// <param name="offset">The number of results to skip before starting to return results. Default is 0.</param>
    /// <param name="fields">Additional fields to include in the JSON response. Default is null.</param>
    public async Task<Paginated<AnimeList>> GetAnimeListAsync(string? query = null, int limit = 100, int offset = 0, IEnumerable<string>? fields = null)
    {
        var url = new ApiUrl("anime", new { limit, offset })
            .AddParamIf("q", query)
            .AddParamIf("fields", StringHelper.ToCommaSeparatedString(fields ?? Enumerable.Empty<string>()));
        
        var error = "Failed to retrieve anime list" + (query != null ? $" for query '{query}'" : string.Empty);
        var response = await _httpClient.GetAsync(url.GetUrl(), error);
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
        var response = await _httpClient.GetAsync(url.GetUrl(), error);
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
        var response = await _httpClient.GetAsync(url.GetUrl(), error);
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
    public async Task<Paginated<AnimeList>> GetAnimeSeasonAsync(int year, string season, string? sort = null, int limit = 100, int offset = 0, IEnumerable<string>? fields = null)
    {
        var url = new ApiUrl($"anime/season/{year}/{season}", new { limit, offset })
            .AddParamIf("fields", StringHelper.ToCommaSeparatedString(fields ?? Enumerable.Empty<string>()))
            .AddParamIf("sort", sort);
        
        var error = $"Failed to retrieve anime season for {season} {year}";
        var response = await _httpClient.GetAsync(url.GetUrl(), error);
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
        var response = await _httpClient.GetAsync(url.GetUrl(), error);
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
        var response = await _httpClient.GetAsync(url.GetUrl(), error);
        var data = Paginated<ForumTopicDetail>.FromJson(response);
        
        return data;
    }
    
    /// <inheritdoc/>
    /// <param name="boardId">The ID of the board to retrieve topics for. Default is null.</param>
    /// <param name="subboardId">The ID of the sub-board to retrieve topics for. Default is null.</param>
    /// <param name="query">The search query to filter topics. Default is null.</param>
    /// <param name="topicUserName">The username of the topic creator. Default is null.</param>
    /// <param name="userName">The username of the topic poster. Default is null.</param>
    public async Task<Paginated<ForumTopic>> GetForumTopicsAsync(int? boardId = null, int? subboardId = null, string? query = null, string? topicUserName = null, string? userName = null, int limit = 100, int offset = 0)
    {
        var url = new ApiUrl("forum/topics", new { limit, offset })
            .AddParamIf("board_id", boardId)
            .AddParamIf("subboard_id", subboardId)
            .AddParamIf("q", query)
            .AddParamIf("topic_user_name", topicUserName)
            .AddParamIf("user_name", userName);
        
        var error = "Failed to retrieve forum topics for"
            + (boardId != null ? $" board ID '{boardId}'." : string.Empty)
            + (subboardId != null ? $" subboard ID '{subboardId}'." : string.Empty)
            + (query != null ? $" query '{query}'." : string.Empty)
            + (topicUserName != null ? $" topic user name '{topicUserName}'." : string.Empty)
            + (userName != null ? $" user name '{userName}'." : string.Empty);
        
        var response = await _httpClient.GetAsync(url.GetUrl(), error);
        var data = Paginated<ForumTopic>.FromJson(response);
        
        return data;
    }
    
    
    #endregion
}