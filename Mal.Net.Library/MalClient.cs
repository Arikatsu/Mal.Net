using System.Text.Json;
using Mal.Net.Utils;
using Mal.Net.Exceptions;
using Mal.Net.Schemas;
using Mal.Net.Schemas.Anime;
using Mal.Net.Schemas.Forum;
using Mal.Net.Schemas.Manga;
using Mal.Net.Schemas.Auth;
using Mal.Net.Services.Base;

namespace Mal.Net;

/// <summary>
/// Represents the main entry point for interacting with the MyAnimeList API.
/// </summary>
public class MalClient : MalClientApiBase
{
    
    private readonly string _clientId;
    private readonly string? _clientSecret;

    /// <summary>
    /// Initializes a new instance of the <see cref="MalClient"/> class using only the client ID.
    /// This constructor is intended for Android, iOS, and Other app types that cannot safely store the client secret.
    /// </summary>
    /// <param name="clientId">The client ID provided by MyAnimeList.</param>
    public MalClient(string clientId)
    {
        _clientId = clientId;
        
        MalHttpClient.SetClientId(clientId);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MalClient"/> class using both the client ID and client secret.
    /// This constructor is intended for Web app types that can safely store the client secret.
    /// </summary>
    /// <param name="clientId">The client ID provided by MyAnimeList.</param>
    /// <param name="clientSecret">The client secret provided by MyAnimeList.</param>
    public MalClient(string clientId, string clientSecret)
    {
        _clientId = clientId;
        _clientSecret = clientSecret;
        
        MalHttpClient.SetClientId(clientId);
    }
    
    
    #region Authentication API Calls
    
    
    /// <summary>
    /// Generates the URL for the OAuth2 authorization endpoint.
    /// </summary>
    /// <param name="state">The state parameter to include in the URL.</param>
    /// <param name="codeVerifier">The code verifier parameter to include in the URL.</param>
    /// <param name="redirectUri">The redirect URI to include in the URL. Leave null if only one redirect URI was specified while creating the MAL API application.</param>
    /// <returns>The URL for the OAuth2 authorization endpoint.</returns>
    public string GenerateAuthUrl(out string state, out string? codeVerifier, string? redirectUri = null)
    {
        state = AuthHelper.GenerateState();
        codeVerifier = AuthHelper.GenerateCodeVerifier();
        
        var codeChallenge = codeVerifier;
        
        var url = new ApiUrl("oauth2/authorize", new
        {
            response_type = "code",
            client_id = _clientId,
            state, 
            code_challenge = codeChallenge, 
            code_challenge_method = "plain"
        }, forAuth: true)
            .AddParamIf("redirect_uri", redirectUri);
        
        return url.GetUrl();
    }
    
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
    public async Task<MalUser> AuthenticateUser(string code, string codeVerifier, string? redirectUri = null, CancellationToken cancellationToken = default)
    {
        var url = new ApiUrl("oauth2/token", forAuth: true);
        
        var keyValuePairs = new List<KeyValuePair<string, string>>
        {
            new("client_id", _clientId),
            new("code", code),
            new("code_verifier", codeVerifier),
            new("grant_type", "authorization_code")
        };
        
        if (!string.IsNullOrEmpty(_clientSecret))
        {
            keyValuePairs.Add(new KeyValuePair<string, string>("client_secret", _clientSecret));
        }
        
        if (!string.IsNullOrEmpty(redirectUri))
        {
            keyValuePairs.Add(new KeyValuePair<string, string>("redirect_uri", redirectUri));
        }
        
        var content = new FormUrlEncodedContent(keyValuePairs);
        var response = await MalHttpClient.PostAsync(url.GetUrlWithoutParams(), content, "Failed to authenticate user", cancellationToken);
        var data = OAuthResponse.FromJson(response);
        
        return new MalUser(data, _clientId, _clientSecret);
    }
    
    
    #endregion
}