namespace Mal.Net.Services
{
    /// <summary>
    /// Defines methods for interacting with anime-related API endpoints.
    /// </summary>
    public interface IAnimeService
    {
        /// <summary>
        /// Retrieves a list of anime based on the provided search query.
        /// </summary>
        /// <param name="query">The search query to filter anime.</param>
        /// <param name="limit">The maximum number of results to return. Default is 100.</param>
        /// <param name="offset">The number of results to skip before starting to return results. Default is 0.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the JSON response from the API.</returns>
        Task<string> GetAnimeListAsync(string query, int limit = 100, int offset = 0);
    }
}