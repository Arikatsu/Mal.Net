using Mal.Net.Services.Interfaces;
using Mal.Net.Utils;
using Mal.Net.Schemas;
using Mal.Net.Schemas.Anime;

namespace Mal.Net.Services.Base;

public class MalUserApiBase : MalClientApiBase, IAnimeUserService
{
    protected MalUserApiBase(string? accessToken, string? tokenType)
        : base(accessToken, tokenType) { }
    
    /// <inheritdoc />
    /// <param name="options">The optional parameters to include in the request. For default values see <see cref="MalRequestOptions"/>.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the asynchronous operation. Default is <see cref="CancellationToken.None"/>.</param>
    public async Task<Paginated<AnimeList>> GetSuggestedAnimeAsync(MalRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        options ??= new MalRequestOptions();
        var url = new ApiUrl("anime/suggestions",
                new { limit = options.Limit, offset = options.Offset, nsfw = options.IncludeNsfw.ToString().ToLower() })
            .AddParamIf("fields", StringHelper.ToCommaSeparatedString(options.Fields));
        
        const string error = "Failed to retrieve suggested anime list";
        var response = await MalHttpClient.GetAsync(url.GetUrl(), error, TokenType, AccessToken, cancellationToken);
        var data = Paginated<AnimeList>.FromJson(response);

        return data;
    }
}