using System.Text.Json;
using Mal.Net.Services;
using Mal.Net.Utils;
using Mal.Net.Schemas;
using Mal.Net.Schemas.Anime;

namespace Mal.Net;

/// <summary>
/// Represents the main entry point for interacting with the MyAnimeList API.
/// </summary>
public class MalClient : IDisposable, IAnimeService
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
}