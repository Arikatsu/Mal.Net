using System.Text.Json;
using Mal.Net.Exceptions;
using Mal.Net.Schemas;
using Mal.Net.Schemas.Forum;

namespace Mal.Net.Services;

/// <summary>
/// Defines methods for interacting with forum-related API endpoints.
/// </summary>
public interface IForumService
{
    /// <summary>
    /// Retrieves a list of forum boards.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a <see cref="Forums"/> object.
    /// </returns>
    /// <exception cref="MalHttpException">Thrown when an HTTP error occurs.</exception>
    /// <exception cref="JsonException">Thrown when an error occurs while deserializing the JSON response.</exception>
    Task<Forums> GetForumBoardsAsync();
    
    /// <summary>
    /// Retrieves a list of forum topics based on the provided topic ID.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a <see cref="Paginated{T}"/> object of <see cref="ForumTopicDetail"/>.
    /// </returns>
    /// <exception cref="MalHttpException">Thrown when an HTTP error occurs.</exception>
    /// <exception cref="JsonException">Thrown when an error occurs while deserializing the JSON response.</exception>
    Task<Paginated<ForumTopicDetail>> GetForumTopicsDetailAsync(int topicId, int limit, int offset);
    
    /// <summary>
    /// Retrieves a list of forum topics based on the provided query parameters.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a <see cref="Paginated{T}"/> object of <see cref="ForumTopic"/>.
    /// </returns>
    /// <exception cref="MalHttpException">Thrown when an HTTP error occurs.</exception>
    /// <exception cref="JsonException">Thrown when an error occurs while deserializing the JSON response.</exception>
    Task<Paginated<ForumTopic>> GetForumTopicsAsync(int? boardId, int? subBoardId, string query, string topicUserName, string userName, int limit, int offset);
}