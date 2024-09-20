using System.Text.Json;
using Mal.Net.Core.Interfaces;
using Mal.Net.Exceptions;

namespace Mal.Net;

/// <summary>
/// Represents the main entry point for interacting with the MyAnimeList API.
/// </summary>
public interface IMalClient : IAnimeResource, IForumResource, IMangaResource
{
    /// <summary>
    /// Generates the URL for the OAuth2 authorization endpoint.
    /// </summary>
    /// <param name="state">The state parameter to include in the URL.</param>
    /// <param name="codeVerifier">The code verifier parameter to include in the URL.</param>
    /// <param name="redirectUri">The redirect URI to include in the URL. Leave null if only one redirect URI was specified while creating the MAL API application.</param>
    /// <returns>The URL for the OAuth2 authorization endpoint.</returns>
    string GenerateAuthorizationUrl(out string state, out string? codeVerifier, string? redirectUri = null);
    
    /// <summary>
    /// Authenticates a user using the provided authorization code.
    /// </summary>
    /// <param name="code">The authorization code to use for authentication.</param>
    /// <param name="codeVerifier">The code verifier used to generate the authorization code.</param>
    /// <param name="redirectUri">The redirect URI used to generate the authorization code. Leave null if only one redirect URI was specified while creating the MAL API application.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the asynchronous operation. Default is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A <see cref="MalUser"/> object representing the authenticated user.</returns>
    /// <exception cref="MalHttpException">Thrown when the request to the MyAnimeList API fails.</exception>
    /// <exception cref="JsonException">Thrown when the response from the MyAnimeList API cannot be deserialized.</exception>
    Task<MalUser> AuthenticateUserAsync(string code, string codeVerifier, string? redirectUri = null, CancellationToken cancellationToken = default);
}