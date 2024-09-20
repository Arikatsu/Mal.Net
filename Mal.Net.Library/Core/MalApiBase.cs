using Mal.Net.Utils;
using Mal.Net.Models;
using Mal.Net.Models.Anime;
using Mal.Net.Models.Forum;
using Mal.Net.Models.Manga;

namespace Mal.Net.Core;

public class MalApiBase
{
    protected static async Task<Paginated<AnimeList>> GetAnimeListCoreAsync(
        KeyValuePair<string, string> header,
        string? query = null, 
        MalRequestOptions? options = null, 
        CancellationToken cancellationToken = default)
    {
        options ??= new MalRequestOptions();
        var url = new ApiUrl("anime", new { limit = options.Limit, offset = options.Offset, nsfw = options.IncludeNsfw.ToString().ToLower() })
            .AddParamIf("q", query)
            .AddParamIf("fields", StringHelper.ToCommaSeparatedString(options.Fields));
        
        var error = "Failed to retrieve anime list" + (query != null ? $" for query '{query}'" : string.Empty);
        var response = await MalHttpClient.GetAsync(url.GetUrl(), header, error, cancellationToken);
        var data = Paginated<AnimeList>.FromJson(response);
        
        return data;
    }
    
    protected static async Task<AnimeNode> GetAnimeDetailsCoreAsync(
        KeyValuePair<string, string> header,
        int animeId, 
        IEnumerable<string>? fields = null, 
        CancellationToken cancellationToken = default)
    {
        var url = new ApiUrl($"anime/{animeId}")
            .AddParamIf("fields", StringHelper.ToCommaSeparatedString(fields ?? Enumerable.Empty<string>()));
        
        var error = $"Failed to retrieve anime details for ID '{animeId}'";
        var response = await MalHttpClient.GetAsync(url.GetUrl(), header, error, cancellationToken);
        var data = AnimeNode.FromJson(response);
        
        return data;
    }
    
    protected static async Task<Paginated<RankedAnimeList>> GetAnimeRankingCoreAsync(
        KeyValuePair<string, string> header,
        string rankingType, 
        MalRequestOptions? options = null, 
        CancellationToken cancellationToken = default)
    {
        options ??= new MalRequestOptions();
        var url = new ApiUrl("anime/ranking", new { ranking_type = rankingType, limit = options.Limit, offset = options.Offset })
            .AddParamIf("fields", StringHelper.ToCommaSeparatedString(options.Fields));
        
        var error = $"Failed to retrieve anime ranking for type '{rankingType}'";
        var response = await MalHttpClient.GetAsync(url.GetUrl(), header, error, cancellationToken);
        var data = Paginated<RankedAnimeList>.FromJson(response);
        
        return data;
    }
    
    protected static async Task<Paginated<AnimeList>> GetAnimeSeasonCoreAsync(
        KeyValuePair<string, string> header,
        int year, 
        string season, 
        string? sort = null, 
        MalRequestOptions? options = null, 
        CancellationToken cancellationToken = default)
    {
        options ??= new MalRequestOptions();
        var url = new ApiUrl($"anime/season/{year}/{season}", new { limit = options.Limit, offset = options.Offset, nsfw = options.IncludeNsfw.ToString().ToLower() })
            .AddParamIf("fields", StringHelper.ToCommaSeparatedString(options.Fields))
            .AddParamIf("sort", sort);
        
        var error = $"Failed to retrieve anime season for {season} {year}";
        var response = await MalHttpClient.GetAsync(url.GetUrl(), header, error, cancellationToken);
        var data = Paginated<AnimeList>.FromJson(response);
        
        return data;
    }
    
    protected static async Task<Paginated<AnimeList>> GetSuggestedAnimeCoreAsync(
        KeyValuePair<string, string> header,
        MalRequestOptions? options = null, 
        CancellationToken cancellationToken = default)
    {
        options ??= new MalRequestOptions();
        var url = new ApiUrl("anime/suggestions",
                new { limit = options.Limit, offset = options.Offset, nsfw = options.IncludeNsfw.ToString().ToLower() })
            .AddParamIf("fields", StringHelper.ToCommaSeparatedString(options.Fields));
        
        const string error = "Failed to retrieve suggested anime list";
        var response = await MalHttpClient.GetAsync(url.GetUrl(), header, error, cancellationToken);
        var data = Paginated<AnimeList>.FromJson(response);

        return data;
    }
    
    protected static async Task<Forums> GetForumBoardsCoreAsync(
        KeyValuePair<string, string> header,
        CancellationToken cancellationToken = default)
    {
        var url = new ApiUrl("forum/boards");
        
        const string error = "Failed to retrieve forum boards";
        var response = await MalHttpClient.GetAsync(url.GetUrl(), header, error, cancellationToken);
        var data = Forums.FromJson(response);
        
        return data;
    }
    
    protected static async Task<Paginated<ForumTopicDetail>> GetForumTopicsDetailCoreAsync(
        KeyValuePair<string, string> header,
        int topicId, 
        MalRequestOptions? options = null, 
        CancellationToken cancellationToken = default)
    {
        options ??= new MalRequestOptions();
        var url = new ApiUrl($"forum/topic/{topicId}", new { limit = options.Limit, offset = options.Offset });
        
        var error = $"Failed to retrieve forum topics for ID '{topicId}'";
        var response = await MalHttpClient.GetAsync(url.GetUrl(), header, error, cancellationToken);
        var data = Paginated<ForumTopicDetail>.FromJson(response);
        
        return data;
    }
    
    protected static async Task<Paginated<ForumTopic>> GetForumTopicsCoreAsync(
        KeyValuePair<string, string> header,
        ForumTopicOptions? options = null, 
        CancellationToken cancellationToken = default)
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
        
        var response = await MalHttpClient.GetAsync(url.GetUrl(), header, error, cancellationToken);
        var data = Paginated<ForumTopic>.FromJson(response);
        
        return data;
    }
    
    protected static async Task<Paginated<MangaList>> GetMangaListCoreAsync(
        KeyValuePair<string, string> header,
        string? query = null, 
        MalRequestOptions? options = null, 
        CancellationToken cancellationToken = default)
    {
        options ??= new MalRequestOptions();
        var url = new ApiUrl("manga", new { limit = options.Limit, offset = options.Offset, nsfw = options.IncludeNsfw.ToString().ToLower() })
            .AddParamIf("q", query)
            .AddParamIf("fields", StringHelper.ToCommaSeparatedString(options.Fields));
        
        var error = "Failed to retrieve manga list" + (query != null ? $" for query '{query}'" : string.Empty);
        var response = await MalHttpClient.GetAsync(url.GetUrl(), header, error, cancellationToken);
        var data = Paginated<MangaList>.FromJson(response);
        
        return data;
    }
    
    protected static async Task<MangaNode> GetMangaDetailsCoreAsync(
        KeyValuePair<string, string> header,
        int mangaId, 
        IEnumerable<string>? fields = null, 
        CancellationToken cancellationToken = default)
    {
        var url = new ApiUrl($"manga/{mangaId}")
            .AddParamIf("fields", StringHelper.ToCommaSeparatedString(fields ?? Enumerable.Empty<string>()));
        
        var error = $"Failed to retrieve manga details for ID '{mangaId}'";
        var response = await MalHttpClient.GetAsync(url.GetUrl(), header, error, cancellationToken);
        var data = MangaNode.FromJson(response);
        
        return data;
    }
    
    protected static async Task<Paginated<RankedMangaList>> GetMangaRankingCoreAsync(
        KeyValuePair<string, string> header,
        string rankingType, 
        MalRequestOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        options ??= new MalRequestOptions();
        var url = new ApiUrl("manga/ranking", new { ranking_type = rankingType, limit = options.Limit, offset = options.Offset })
            .AddParamIf("fields", StringHelper.ToCommaSeparatedString(options.Fields));
        
        var error = $"Failed to retrieve manga ranking for type '{rankingType}'";
        Console.WriteLine(url.GetUrl());
        var response = await MalHttpClient.GetAsync(url.GetUrl(), header, error, cancellationToken);
        var data = Paginated<RankedMangaList>.FromJson(response);
        
        return data;
    }
}