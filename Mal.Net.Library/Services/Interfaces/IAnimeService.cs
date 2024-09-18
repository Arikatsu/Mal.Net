using System.Text.Json;
using Mal.Net.Exceptions;
using Mal.Net.Schemas;
using Mal.Net.Schemas.Anime;

namespace Mal.Net.Services.Interfaces;

/// <summary>
/// Defines methods for interacting with anime-related API endpoints.
/// </summary>
public interface IAnimeService
{
    /// <summary>
    /// Retrieves a list of anime based on the provided search query.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a <see cref="Paginated{T}"/> object of <see cref="AnimeList"/>.
    /// </returns>
    /// <exception cref="MalHttpException">Thrown when an HTTP error occurs.</exception>
    /// <exception cref="JsonException">Thrown when an error occurs while deserializing the JSON response.</exception>
    Task<Paginated<AnimeList>> GetAnimeListAsync(string query, MalRequestOptions options, CancellationToken cancellationToken);
    
    /// <summary>
    /// Retrieves details for a specific anime based on its ID.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains an <see cref="AnimeNode"/> object.
    /// </returns>
    /// <exception cref="MalHttpException">Thrown when an HTTP error occurs.</exception>
    /// <exception cref="JsonException">Thrown when an error occurs while deserializing the JSON response.</exception>
    Task<AnimeNode> GetAnimeDetailsAsync(int animeId, IEnumerable<string>? fields, CancellationToken cancellationToken);
    
    /// <summary>
    /// Retrieves a list of anime based on the current ranking type.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a <see cref="Paginated{T}"/> object of <see cref="RankedAnimeList"/>.
    /// </returns>
    /// <exception cref="MalHttpException">Thrown when an HTTP error occurs.</exception>
    /// <exception cref="JsonException">Thrown when an error occurs while deserializing the JSON response.</exception>
    Task<Paginated<RankedAnimeList>> GetAnimeRankingAsync(string rankingType, MalRequestOptions options, CancellationToken cancellationToken);
    
    /// <summary>
    /// Retrieves a list of anime based on the current season.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a <see cref="Paginated{T}"/> object of <see cref="AnimeList"/>.
    /// </returns>
    /// <exception cref="MalHttpException">Thrown when an HTTP error occurs.</exception>
    /// <exception cref="JsonException">Thrown when an error occurs while deserializing the JSON response.</exception>
    Task<Paginated<AnimeList>> GetAnimeSeasonAsync(int year, string season, string sort, MalRequestOptions options, CancellationToken cancellationToken);
}