using System.Text.Json;
using Mal.Net.Exceptions;
using Mal.Net.Schemas;
using Mal.Net.Schemas.Anime;

namespace Mal.Net.Services.Interfaces;

/// <summary>
/// Defines methods for interacting with anime-related API endpoints that require user authentication.
/// </summary>
public interface IAnimeUserService : IAnimeService
{
    /// <summary>
    /// Retrieves a list of anime based on the user's anime list.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a <see cref="Paginated{T}"/> object of <see cref="AnimeList"/>.
    /// </returns>
    /// <exception cref="MalHttpException">Thrown when an HTTP error occurs.</exception>
    /// <exception cref="JsonException">Thrown when an error occurs while deserializing the JSON response.</exception>
    Task<Paginated<AnimeList>> GetSuggestedAnimeAsync(MalRequestOptions options, CancellationToken cancellationToken);
}