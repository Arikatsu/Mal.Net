using System.Text.Json;
using Mal.Net.Exceptions;
using Mal.Net.Models;
using Mal.Net.Models.Anime;

namespace Mal.Net.Core.Interfaces;

/// <summary>
/// Defines methods for interacting with anime-related API endpoints.
/// </summary>
public interface IAnimeResource
{
    /// <summary>
    /// Retrieves a list of anime based on the provided search query.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a <see cref="Paginated{T}"/> object of <see cref="AnimeList"/>.
    /// </returns>
    /// <param name="query">The search query to filter anime.</param>
    /// <param name="options">The optional parameters to include in the request. For default values see <see cref="MalRequestOptions"/>.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the asynchronous operation. Default is <see cref="CancellationToken.None"/>.</param>
    /// <exception cref="MalHttpException">Thrown when an HTTP error occurs.</exception>
    /// <exception cref="JsonException">Thrown when an error occurs while deserializing the JSON response.</exception>
    Task<Paginated<AnimeList>> GetAnimeListAsync(string? query = null, MalRequestOptions? options = null, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Retrieves details for a specific anime based on its ID.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains an <see cref="AnimeNode"/> object.
    /// </returns>
    /// <param name="animeId">The ID of the anime to retrieve details for.</param>
    /// <param name="fields">Additional fields to include in the JSON response. Default is null.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the asynchronous operation. Default is <see cref="CancellationToken.None"/>.</param>
    /// <exception cref="MalHttpException">Thrown when an HTTP error occurs.</exception>
    /// <exception cref="JsonException">Thrown when an error occurs while deserializing the JSON response.</exception>
    Task<AnimeNode> GetAnimeDetailsAsync(int animeId, IEnumerable<string>? fields = null, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Retrieves a list of anime based on the current ranking type.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a <see cref="Paginated{T}"/> object of <see cref="RankedAnimeList"/>.
    /// </returns>
    /// <param name="rankingType">The type of ranking to retrieve. Valid types include "all", "airing", "upcoming", "tv", "ova", "movie", "special", "bypopularity", "favorite".</param>
    /// <param name="options">The optional parameters to include in the request. For default values see <see cref="MalRequestOptions"/>.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the asynchronous operation. Default is <see cref="CancellationToken.None"/>.</param>
    /// <exception cref="MalHttpException">Thrown when an HTTP error occurs.</exception>
    /// <exception cref="JsonException">Thrown when an error occurs while deserializing the JSON response.</exception>
    Task<Paginated<RankedAnimeList>> GetAnimeRankingAsync(string rankingType, MalRequestOptions? options = null, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Retrieves a list of anime based on the current season.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a <see cref="Paginated{T}"/> object of <see cref="AnimeList"/>.
    /// </returns>
    /// <param name="year">The year to retrieve the season for.</param>
    /// <param name="season">The season to retrieve. Valid seasons include "winter", "spring", "summer", "fall".</param>
    /// <param name="sort">The sort order to use. Valid options include "anime_score", "anime_num_list_users".</param>
    /// <param name="options">The optional parameters to include in the request. For default values see <see cref="MalRequestOptions"/>.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the asynchronous operation. Default is <see cref="CancellationToken.None"/>.</param>
    /// <exception cref="MalHttpException">Thrown when an HTTP error occurs.</exception>
    /// <exception cref="JsonException">Thrown when an error occurs while deserializing the JSON response.</exception>
    Task<Paginated<AnimeList>> GetAnimeSeasonAsync(int year, string season, string? sort = null, MalRequestOptions? options = null, CancellationToken cancellationToken = default);
}