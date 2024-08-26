using Mal.Net.Services;
using Mal.Net.Utils;

namespace Mal.Net
{
    /// <summary>
    /// Represents the main entry point for interacting with the MyAnimeList API.
    /// </summary>
    public class MalClient : IDisposable, IAnimeService
    {
        internal readonly MalHttpClient HttpClient;
        
        protected readonly string _clientId;
        protected readonly string? _clientSecret;

        /// <summary>
        /// Initializes a new instance of the <see cref="MalClient"/> class using only the client ID.
        /// This constructor is intended for Android, iOS, and Other app types that cannot safely store the client secret.
        /// </summary>
        /// <param name="clientId">The client ID provided by MyAnimeList.</param>
        public MalClient(string clientId)
        {
            _clientId = clientId;

            HttpClient = new MalHttpClient(clientId);
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

            HttpClient = new MalHttpClient(clientId);
        }

        /// <summary>
        /// Releases all resources used by the current instance of the <see cref="MalClient"/> class.
        /// </summary>
        public void Dispose()
        {
            HttpClient?.Dispose();

            GC.SuppressFinalize(this);
        }

        
        #region Anime API Calls

        /// <inheritdoc/>
        public async Task<string> GetAnimeListAsync(string query, int limit = 100, int offset = 0)
        {
            string url = $"https://api.myanimelist.net/v2/anime?q={query}&limit={limit}&offset={offset}";
            return await HttpClient.GetAsync(url);
        }

        #endregion

        
        
    }
}
