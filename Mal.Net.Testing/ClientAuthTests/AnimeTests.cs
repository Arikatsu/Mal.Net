using System.Text.Json;
using Xunit.Abstractions;

namespace Mal.Net.Testing.ClientAuthTests;

public class AnimeTests : TestsBase
{
    public AnimeTests(ITestOutputHelper output) : base(output) { }
    
    [Fact]
    public async Task AnimeListTest()
    {
        IEnumerable<string> fields = new List<string> { AnimeFields.Rating, AnimeFields.StartDate };

        var animeList = await Client.GetAnimeListAsync("oregairu", 
            new MalRequestOptions
            {
                Limit = 10,
                Fields = fields
            }, 
            CancellationToken.None);

        var json = JsonSerializer.Serialize(animeList, Options);
        Output.WriteLine(json);

        Assert.NotNull(animeList);
    }
    
    [Fact]
    public async Task AnimeDetailsTest()
    {
        IEnumerable<string> fields = new List<string> { AnimeFields.Rating, AnimeFields.StartDate };

        var animeDetails = await Client.GetAnimeDetailsAsync(33161, fields, CancellationToken.None);

        var json = JsonSerializer.Serialize(animeDetails, Options);
        Output.WriteLine(json);

        Assert.NotNull(animeDetails);
    }
    
    [Fact]
    public async Task AnimeRankingTest()
    {
        var animeRanking = await Client.GetAnimeRankingAsync("airing", 
            new MalRequestOptions
            {
                Limit = 10
            }, 
            CancellationToken.None);

        var json = JsonSerializer.Serialize(animeRanking, Options);
        Output.WriteLine(json);

        Assert.NotNull(animeRanking);
    }
    
    [Fact]
    public async Task AnimeSeasonTest()
    {
        var animeSeason = await Client.GetAnimeSeasonAsync(2024, "summer", "anime_score", 
            new MalRequestOptions
            {
                Limit = 10
            });

        var json = JsonSerializer.Serialize(animeSeason, Options);
        Output.WriteLine(json);

        Assert.NotNull(animeSeason);
    }
    
}