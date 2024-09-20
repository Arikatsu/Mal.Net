using System.Text.Json;
using Mal.Net.Exceptions;
using Mal.Net.Models;
using Mal.Net.Models.Manga;

namespace Mal.Net.Core.Interfaces;

public interface IMangaResource
{
    /// <summary>
    /// Retrieves a list of manga based on the provided query parameters.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a <see cref="Paginated{T}"/> object of <see cref="MangaList"/>.
    /// </returns>
    /// <param name="query">The search query to filter manga.</param>
    /// <param name="options">The optional parameters to include in the request. For default values see <see cref="MalRequestOptions"/>.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the asynchronous operation. Default is <see cref="CancellationToken.None"/>.</param>
    /// <exception cref="MalHttpException">Thrown when an HTTP error occurs.</exception>
    /// <exception cref="JsonException">Thrown when an error occurs while deserializing the JSON response.</exception>
    Task<Paginated<MangaList>> GetMangaListAsync(string? query = null, MalRequestOptions? options = null, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Retrieves details for a specific manga based on its ID.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a <see cref="MangaNode"/> object.
    /// </returns>
    /// <param name="mangaId">The ID of the manga to retrieve details for.</param>
    /// <param name="fields">Additional fields to include in the JSON response. Default is null.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the asynchronous operation. Default is <see cref="CancellationToken.None"/>.</param>
    /// <exception cref="MalHttpException">Thrown when an HTTP error occurs.</exception>
    /// <exception cref="JsonException">Thrown when an error occurs while deserializing the JSON response.</exception>
    Task<MangaNode> GetMangaDetailsAsync(int mangaId, IEnumerable<string>? fields = null, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Retrieves a list of manga based on the current ranking type.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a <see cref="Paginated{T}"/> object of <see cref="RankedMangaList"/>.
    /// </returns>
    /// <param name="rankingType">The type of ranking to retrieve. Valid types include "all", "manga", "novels", "oneshots", "doujin", "manhwa", "manhua", "bypopularity", "favorite".</param>
    /// <param name="options">The optional parameters to include in the request. For default values see <see cref="MalRequestOptions"/>.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the asynchronous operation. Default is <see cref="CancellationToken.None"/>.</param>
    /// <exception cref="MalHttpException">Thrown when an HTTP error occurs.</exception>
    /// <exception cref="JsonException">Thrown when an error occurs while deserializing the JSON response.</exception>
    Task<Paginated<RankedMangaList>> GetMangaRankingAsync(string rankingType, MalRequestOptions? options = null, CancellationToken cancellationToken = default);
}