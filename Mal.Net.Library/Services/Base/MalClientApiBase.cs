using Mal.Net.Services.Interfaces;
using Mal.Net.Utils;
using Mal.Net.Schemas;
using Mal.Net.Schemas.Anime;
using Mal.Net.Schemas.Forum;
using Mal.Net.Schemas.Manga;

namespace Mal.Net.Services.Base;

public class MalClientApiBase : IAnimeService, IForumService, IMangaService
{
    protected string? AccessToken;
    protected string? TokenType;
    
    protected MalClientApiBase(string? accessToken = null, string? tokenType = null)
    {
        AccessToken = accessToken;
        TokenType = tokenType;
    }
    
    protected void SetAccessToken(string? accessToken, string? tokenType)
    {
        AccessToken = accessToken;
        TokenType = tokenType;
    }
    
    #region Anime API Calls

    
    /// <inheritdoc/>
    /// <param name="query">The search query to filter anime.</param>
    /// <param name="options">The optional parameters to include in the request. For default values see <see cref="MalRequestOptions"/>.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the asynchronous operation. Default is <see cref="CancellationToken.None"/>.</param>
    public async Task<Paginated<AnimeList>> GetAnimeListAsync(string? query = null, MalRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        options ??= new MalRequestOptions();
        var url = new ApiUrl("anime", new { limit = options.Limit, offset = options.Offset, nsfw = options.IncludeNsfw.ToString().ToLower() })
            .AddParamIf("q", query)
            .AddParamIf("fields", StringHelper.ToCommaSeparatedString(options.Fields));
        
        var error = "Failed to retrieve anime list" + (query != null ? $" for query '{query}'" : string.Empty);
        var response = await MalHttpClient.GetAsync(url.GetUrl(), error, TokenType, AccessToken, cancellationToken);
        var data = Paginated<AnimeList>.FromJson(response);
        
        return data;
    }
    
    /// <inheritdoc/>
    /// <param name="animeId">The ID of the anime to retrieve details for.</param>
    /// <param name="fields">Additional fields to include in the JSON response. Default is null.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the asynchronous operation. Default is <see cref="CancellationToken.None"/>.</param>
    public async Task<AnimeNode> GetAnimeDetailsAsync(int animeId, IEnumerable<string>? fields = null, CancellationToken cancellationToken = default)
    {
        var url = new ApiUrl($"anime/{animeId}")
            .AddParamIf("fields", StringHelper.ToCommaSeparatedString(fields ?? Enumerable.Empty<string>()));
        
        var error = $"Failed to retrieve anime details for ID '{animeId}'";
        var response = await MalHttpClient.GetAsync(url.GetUrl(), error, TokenType, AccessToken, cancellationToken);
        var data = AnimeNode.FromJson(response);
        
        return data;
    }
    
    /// <inheritdoc/>
    /// <param name="rankingType">The type of ranking to retrieve. Valid types include "all", "airing", "upcoming", "tv", "ova", "movie", "special", "bypopularity", "favorite".</param>
    /// <param name="options">The optional parameters to include in the request. For default values see <see cref="MalRequestOptions"/>.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the asynchronous operation. Default is <see cref="CancellationToken.None"/>.</param>
    public async Task<Paginated<RankedAnimeList>> GetAnimeRankingAsync(string rankingType, MalRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        options ??= new MalRequestOptions();
        var url = new ApiUrl("anime/ranking", new { ranking_type = rankingType, limit = options.Limit, offset = options.Offset })
            .AddParamIf("fields", StringHelper.ToCommaSeparatedString(options.Fields));
        
        var error = $"Failed to retrieve anime ranking for type '{rankingType}'";
        var response = await MalHttpClient.GetAsync(url.GetUrl(), error, TokenType, AccessToken, cancellationToken);
        var data = Paginated<RankedAnimeList>.FromJson(response);
        
        return data;
    }

    /// <inheritdoc/>
    /// <param name="year">The year to retrieve the season for.</param>
    /// <param name="season">The season to retrieve. Valid seasons include "winter", "spring", "summer", "fall".</param>
    /// <param name="sort">The sort order to use. Valid options include "anime_score", "anime_num_list_users".</param>
    /// <param name="options">The optional parameters to include in the request. For default values see <see cref="MalRequestOptions"/>.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the asynchronous operation. Default is <see cref="CancellationToken.None"/>.</param>
    public async Task<Paginated<AnimeList>> GetAnimeSeasonAsync(int year, string season, string? sort = null, MalRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        options ??= new MalRequestOptions();
        var url = new ApiUrl($"anime/season/{year}/{season}", new { limit = options.Limit, offset = options.Offset, nsfw = options.IncludeNsfw.ToString().ToLower() })
            .AddParamIf("fields", StringHelper.ToCommaSeparatedString(options.Fields))
            .AddParamIf("sort", sort);
        
        var error = $"Failed to retrieve anime season for {season} {year}";
        var response = await MalHttpClient.GetAsync(url.GetUrl(), error, TokenType, AccessToken, cancellationToken);
        var data = Paginated<AnimeList>.FromJson(response);
        
        return data;
    }


    #endregion
    
    
    #region Forum API Calls
    
    
    /// <inheritdoc/>
    /// <param name="cancellationToken">The cancellation token to cancel the asynchronous operation. Default is <see cref="CancellationToken.None"/>.</param>
    public async Task<Forums> GetForumBoardsAsync(CancellationToken cancellationToken = default)
    {
        var url = new ApiUrl("forum/boards");
        
        const string error = "Failed to retrieve forum boards";
        var response = await MalHttpClient.GetAsync(url.GetUrl(), error, TokenType, AccessToken, cancellationToken);
        var data = Forums.FromJson(response);
        
        return data;
    }
    
    /// <inheritdoc/>
    /// <param name="topicId">The ID of the topic to retrieve details for.</param>
    /// <param name="options">The optional parameters to include in the request. For default values see <see cref="MalRequestOptions"/>.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the asynchronous operation. Default is <see cref="CancellationToken.None"/>.</param>
    public async Task<Paginated<ForumTopicDetail>> GetForumTopicsDetailAsync(int topicId, MalRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        options ??= new MalRequestOptions();
        var url = new ApiUrl($"forum/topic/{topicId}", new { limit = options.Limit, offset = options.Offset });
        
        var error = $"Failed to retrieve forum topics for ID '{topicId}'";
        var response = await MalHttpClient.GetAsync(url.GetUrl(), error, TokenType, AccessToken, cancellationToken);
        var data = Paginated<ForumTopicDetail>.FromJson(response);
        
        return data;
    }
    
    /// <inheritdoc/>
    /// <param name="options">The optional parameters to include in the request. For default values see <see cref="ForumTopicOptions"/>.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the asynchronous operation. Default is <see cref="CancellationToken.None"/>.</param>
    public async Task<Paginated<ForumTopic>> GetForumTopicsAsync(ForumTopicOptions? options = null, CancellationToken cancellationToken = default)
    {
        options ??= new ForumTopicOptions();
        var url = new ApiUrl("forum/topics", new { limit = options.Limit, offset = options.Offset })
            .AddParamIf("board_id", options.BoardId)
            .AddParamIf("subboard_id", options.SubBoardId)
            .AddParamIf("q", options.Query)
            .AddParamIf("topic_user_name", options.TopicUserName)
            .AddParamIf("user_name", options.UserName);
        
        var error = "Failed to retrieve forum topics for"
            + (options.BoardId != null ? $" board ID '{options.BoardId}'." : string.Empty)
            + (options.SubBoardId != null ? $" sub-board ID '{options.SubBoardId}'." : string.Empty)
            + (options.Query != null ? $" query '{options.Query}'." : string.Empty)
            + (options.TopicUserName != null ? $" topic user name '{options.TopicUserName}'." : string.Empty)
            + (options.UserName != null ? $" user name '{options.UserName}'." : string.Empty);
        
        var response = await MalHttpClient.GetAsync(url.GetUrl(), error, TokenType, AccessToken, cancellationToken);
        var data = Paginated<ForumTopic>.FromJson(response);
        
        return data;
    }
    
    
    #endregion
    
    
    #region Manga API Calls
    
    
    /// <inheritdoc/>
    /// <param name="query">The search query to filter manga.</param>
    /// <param name="options">The optional parameters to include in the request. For default values see <see cref="MalRequestOptions"/>.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the asynchronous operation. Default is <see cref="CancellationToken.None"/>.</param>
    public async Task<Paginated<MangaList>> GetMangaListAsync(string? query = null, MalRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        options ??= new MalRequestOptions();
        var url = new ApiUrl("manga", new { limit = options.Limit, offset = options.Offset, nsfw = options.IncludeNsfw.ToString().ToLower() })
            .AddParamIf("q", query)
            .AddParamIf("fields", StringHelper.ToCommaSeparatedString(options.Fields));
        
        var error = "Failed to retrieve manga list" + (query != null ? $" for query '{query}'" : string.Empty);
        var response = await MalHttpClient.GetAsync(url.GetUrl(), error, TokenType, AccessToken, cancellationToken);
        var data = Paginated<MangaList>.FromJson(response);
        
        return data;
    }
    
    /// <inheritdoc/>
    /// <param name="mangaId">The ID of the manga to retrieve details for.</param>
    /// <param name="fields">Additional fields to include in the JSON response. Default is null.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the asynchronous operation. Default is <see cref="CancellationToken.None"/>.</param>
    public async Task<MangaNode> GetMangaDetailsAsync(int mangaId, IEnumerable<string>? fields = null, CancellationToken cancellationToken = default)
    {
        var url = new ApiUrl($"manga/{mangaId}")
            .AddParamIf("fields", StringHelper.ToCommaSeparatedString(fields ?? Enumerable.Empty<string>()));
        
        var error = $"Failed to retrieve manga details for ID '{mangaId}'";
        var response = await MalHttpClient.GetAsync(url.GetUrl(), error, TokenType, AccessToken, cancellationToken);
        var data = MangaNode.FromJson(response);
        
        return data;
    }
    
    /// <inheritdoc/>
    /// <param name="rankingType">The type of ranking to retrieve. Valid types include "all", "manga", "novels", "oneshots", "doujin", "manhwa", "manhua", "bypopularity", "favorite".</param>
    /// <param name="options">The optional parameters to include in the request. For default values see <see cref="MalRequestOptions"/>.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the asynchronous operation. Default is <see cref="CancellationToken.None"/>.</param>
    public async Task<Paginated<RankedMangaList>> GetMangaRankingAsync(string rankingType, MalRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        options ??= new MalRequestOptions();
        var url = new ApiUrl("manga/ranking", new { ranking_type = rankingType, limit = options.Limit, offset = options.Offset })
            .AddParamIf("fields", StringHelper.ToCommaSeparatedString(options.Fields));
        
        var error = $"Failed to retrieve manga ranking for type '{rankingType}'";
        Console.WriteLine(url.GetUrl());
        var response = await MalHttpClient.GetAsync(url.GetUrl(), error, TokenType, AccessToken, cancellationToken);
        var data = Paginated<RankedMangaList>.FromJson(response);
        
        return data;
    }
    
    
    #endregion
}