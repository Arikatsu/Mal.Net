using Mal.Net.Services.Interfaces;
using Mal.Net.Utils;
using Mal.Net.Schemas;
using Mal.Net.Schemas.Anime;
using Mal.Net.Schemas.Forum;
using Mal.Net.Schemas.Manga;

namespace Mal.Net.Services.Base;

public class MalUserApiBase : MalClientApiBase, IAnimeUserService
{
    protected MalUserApiBase(string? accessToken, string? tokenType)
        : base(accessToken, tokenType) { }
    
    /// <inheritdoc />
    /// <param name="limit">The maximum number of results to return. Default is 100.</param>
    /// <param name="offset">The number of results to skip before starting to return results. Default is 0.</param>
    /// <param name="fields">Additional fields to include in the JSON response. Default is null.</param>
    /// <param name="includeNsfw">Whether to include NSFW content in the results. Default is false.</param>
    public async Task<Paginated<AnimeList>> GetSuggestedAnimeAsync(int limit = 100, int offset = 0, IEnumerable<string>? fields = null, bool includeNsfw = false)
    {
        var url = new ApiUrl("anime/suggestions", new { limit, offset, nsfw = includeNsfw.ToString().ToLower() })
            .AddParamIf("fields", StringHelper.ToCommaSeparatedString(fields ?? Enumerable.Empty<string>()));
        
        const string error = "Failed to retrieve suggested anime list";
        var response = await MalHttpClient.GetAsync(url.GetUrl(), error, TokenType, AccessToken);
        var data = Paginated<AnimeList>.FromJson(response);

        return data;
    }
}