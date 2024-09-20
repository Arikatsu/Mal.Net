using System.Text.Json;
using Mal.Net.Exceptions;
using Mal.Net.Models;
using Mal.Net.Models.Forum;

namespace Mal.Net.Core.Interfaces;

/// <summary>
/// Defines methods for interacting with forum-related API endpoints.
/// </summary>
public interface IForumResource
{
    /// <summary>
    /// Retrieves a list of forum boards.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a <see cref="Forums"/> object.
    /// </returns>
    /// <param name="cancellationToken">The cancellation token to cancel the asynchronous operation. Default is <see cref="CancellationToken.None"/>.</param>
    /// <exception cref="MalHttpException">Thrown when an HTTP error occurs.</exception>
    /// <exception cref="JsonException">Thrown when an error occurs while deserializing the JSON response.</exception>
    Task<Forums> GetForumBoardsAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Retrieves a list of forum topics based on the provided topic ID.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a <see cref="Paginated{T}"/> object of <see cref="ForumTopicDetail"/>.
    /// </returns>
    /// <param name="topicId">The ID of the topic to retrieve details for.</param>
    /// <param name="options">The optional parameters to include in the request. For default values see <see cref="MalRequestOptions"/>.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the asynchronous operation. Default is <see cref="CancellationToken.None"/>.</param>
    /// <exception cref="MalHttpException">Thrown when an HTTP error occurs.</exception>
    /// <exception cref="JsonException">Thrown when an error occurs while deserializing the JSON response.</exception>
    Task<Paginated<ForumTopicDetail>> GetForumTopicsDetailAsync(int topicId, MalRequestOptions? options = null, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Retrieves a list of forum topics based on the provided query parameters.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a <see cref="Paginated{T}"/> object of <see cref="ForumTopic"/>.
    /// </returns>
    /// <param name="options">The optional parameters to include in the request. For default values see <see cref="ForumTopicOptions"/>.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the asynchronous operation. Default is <see cref="CancellationToken.None"/>.</param>
    /// <exception cref="MalHttpException">Thrown when an HTTP error occurs.</exception>
    /// <exception cref="JsonException">Thrown when an error occurs while deserializing the JSON response.</exception>
    Task<Paginated<ForumTopic>> GetForumTopicsAsync(ForumTopicOptions? options = null, CancellationToken cancellationToken = default);
}