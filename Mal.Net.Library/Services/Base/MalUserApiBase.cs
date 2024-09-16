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

    public async Task<Paginated<AnimeList>> GetSuggestedAnimeAsync(int limit = 100, int offset = 0, IEnumerable<string>? fields = null, bool includeNsfw = false)
    {
        var url = new ApiUrl("anime/suggestions", new { limit, offset, nsfw = includeNsfw })
            .AddParamIf("fields", StringHelper.ToCommaSeparatedString(fields ?? Enumerable.Empty<string>()));
        
        const string error = "Failed to retrieve suggested anime list";
        var response = await MalHttpClient.GetAsync(url.GetUrl(), error, TokenType, AccessToken);
        var data = Paginated<AnimeList>.FromJson(response);

        return data;
    }
}