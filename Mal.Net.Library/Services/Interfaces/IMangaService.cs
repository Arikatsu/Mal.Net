using System.Text.Json;
using Mal.Net.Exceptions;
using Mal.Net.Schemas;
using Mal.Net.Schemas.Manga;

namespace Mal.Net.Services.Interfaces;

public interface IMangaService
{
    /// <summary>
    /// Retrieves a list of manga based on the provided query parameters.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a <see cref="Paginated{T}"/> object of <see cref="MangaList"/>.
    /// </returns>
    /// <exception cref="MalHttpException">Thrown when an HTTP error occurs.</exception>
    /// <exception cref="JsonException">Thrown when an error occurs while deserializing the JSON response.</exception>
    Task<Paginated<MangaList>> GetMangaListAsync(string query, int limit, int offset, IEnumerable<string> fields, bool includeNsfw);
    
    /// <summary>
    /// Retrieves details for a specific manga based on its ID.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a <see cref="MangaNode"/> object.
    /// </returns>
    /// <exception cref="MalHttpException">Thrown when an HTTP error occurs.</exception>
    /// <exception cref="JsonException">Thrown when an error occurs while deserializing the JSON response.</exception>
    Task<MangaNode> GetMangaDetailsAsync(int mangaId, IEnumerable<string>? fields);
    
    /// <summary>
    /// Retrieves a list of manga based on the current ranking type.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a <see cref="Paginated{T}"/> object of <see cref="RankedMangaList"/>.
    /// </returns>
    /// <exception cref="MalHttpException">Thrown when an HTTP error occurs.</exception>
    /// <exception cref="JsonException">Thrown when an error occurs while deserializing the JSON response.</exception>
    Task<Paginated<RankedMangaList>> GetMangaRankingAsync(string rankingType, int limit, int offset, IEnumerable<string> fields);
}