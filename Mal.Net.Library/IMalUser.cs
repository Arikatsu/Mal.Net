using System.Text.Json;
using Mal.Net.Core.Interfaces;
using Mal.Net.Exceptions;

namespace Mal.Net;

/// <summary>
/// Represents a user that has been authenticated with MyAnimeList.
/// </summary>
public interface IMalUser : IAnimeUserResource, IForumResource, IMangaResource
{
    /// <summary>
    /// The access token used for authenticated API calls.
    /// </summary>
    string? AccessToken { get; }

    /// <summary>
    /// The refresh token used to obtain new access tokens.
    /// </summary>
    string? RefreshToken { get; }

    /// <summary>
    /// The type of the token (e.g., Bearer).
    /// </summary>
    string? TokenType { get; }

    /// <summary>
    /// The expiration time of the current access token.
    /// </summary>
    DateTime AccessTokenExpiresAt { get; }
    
    /// <summary>
    /// Determines whether the access token has expired.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if the access token has expired; otherwise, <see langword="false"/>.
    /// </returns>
    bool IsAccessTokenExpired();
    
    /// <summary>
    /// Refreshes the access token.
    /// </summary>
    /// <remarks>
    /// If the access token has not expired, this method will return the current user.
    /// </remarks>
    /// <param name="force">Whether to force the refresh of the access token.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the asynchronous operation. Default is <see cref="CancellationToken.None"/>.</param>
    /// <returns>The refreshed user.</returns>
    /// <exception cref="MalHttpException">Thrown when the request to refresh the access token fails.</exception>
    /// <exception cref="JsonException">Thrown when the response from the server is not valid JSON.</exception>
    Task<IMalUser> RefreshAccessTokenAsync(bool force = false, CancellationToken cancellationToken = default);
}