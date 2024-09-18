using System.Text.Json;
using Xunit.Abstractions;

namespace Mal.Net.Testing.MainAuthTests;

public class AnimeTests : TestsBase
{
    public AnimeTests(ITestOutputHelper output) : base(output) {}

    [Fact]
    public async Task SuggestedAnimeTest()
    {
        IEnumerable<string> fields = new List<string> { AnimeFields.Rating, AnimeFields.StartDate };
        
        var suggestedAnime = await User.GetSuggestedAnimeAsync(
            new MalRequestOptions
            {
                Limit = 10,
                Fields = fields,
                IncludeNsfw = true
            }, 
            CancellationToken.None);
        
        var json = JsonSerializer.Serialize(suggestedAnime, Options);
        Output.WriteLine(json);
        
        Assert.NotNull(suggestedAnime);
    }
}