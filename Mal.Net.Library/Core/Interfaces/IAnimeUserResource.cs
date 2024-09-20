using System.Text.Json;
using Mal.Net.Exceptions;
using Mal.Net.Models;
using Mal.Net.Models.Anime;

namespace Mal.Net.Core.Interfaces;

public interface IAnimeUserResource : IAnimeResource
{
    /// <summary>
    /// Retrieves a list of anime based on the user's anime list.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a <see cref="Paginated{T}"/> object of <see cref="AnimeList"/>.
    /// </returns>
    /// <param name="options">The optional parameters to include in the request. For default values see <see cref="MalRequestOptions"/>.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the asynchronous operation. Default is <see cref="CancellationToken.None"/>.</param>
    /// <exception cref="MalHttpException">Thrown when an HTTP error occurs.</exception>
    /// <exception cref="JsonException">Thrown when an error occurs while deserializing the JSON response.</exception>
    Task<Paginated<AnimeList>> GetSuggestedAnimeAsync(MalRequestOptions? options = null, CancellationToken cancellationToken = default);
}