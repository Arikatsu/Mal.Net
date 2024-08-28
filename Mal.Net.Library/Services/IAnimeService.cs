using System.Text.Json;
using Mal.Net.Schemas;
using Mal.Net.Schemas.Anime;

namespace Mal.Net.Services;

/// <summary>
/// Defines methods for interacting with anime-related API endpoints.
/// </summary>
public interface IAnimeService
{
    /// <summary>
    /// Retrieves a list of anime based on the provided search query.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the JSON response from the API.
    /// </returns>
    Task<Paginated<AnimeList>> GetAnimeListAsync(string query, int limit, int offset, IEnumerable<string>? fields);
    
    /// <summary>
    /// Retrieves details for a specific anime based on its ID.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the JSON response from the API.
    /// </returns>
    Task<AnimeNode> GetAnimeDetailsAsync(int animeId, IEnumerable<string>? fields);
}