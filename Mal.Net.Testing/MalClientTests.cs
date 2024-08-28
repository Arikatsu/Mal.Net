using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;

namespace Mal.Net.Testing;

public class MalClientTests
{
    private readonly JsonSerializerOptions _options = new() { WriteIndented = true };
    private readonly ITestOutputHelper _output;
    private readonly MalClient _client;

    public MalClientTests(ITestOutputHelper output)
    {
        Console.SetOut(new LogConverter(output));

        var builder = new ConfigurationBuilder()
            .AddUserSecrets<MalClientTests>()
            .Build();

        IConfiguration configuration = builder;
        
        _output = output;
        _client = new MalClient(configuration["Secrets:ClientId"]!, configuration["Secrets:ClientSecret"]!);
    }

    [Fact]
    public async Task AnimeListTest()
    {
        IEnumerable<string> fields = new List<string> { AnimeFields.Rating, AnimeFields.StartDate };

        var animeList = await _client.GetAnimeListAsync("oregairu", 1, 1, fields);

        var json = JsonSerializer.Serialize(animeList, _options);
        _output.WriteLine(json);

        Assert.NotNull(animeList);
    }
    
    [Fact]
    public async Task AnimeDetailsTest()
    {
        IEnumerable<string> fields = new List<string> { AnimeFields.Rating, AnimeFields.StartDate };

        var animeDetails = await _client.GetAnimeDetailsAsync(33161, fields);

        var json = JsonSerializer.Serialize(animeDetails, _options);
        _output.WriteLine(json);

        Assert.NotNull(animeDetails);
    }
    
    [Fact]
    public async Task AnimeRankingTest()
    {

        var animeRanking = await _client.GetAnimeRankingAsync("airing", 10);

        var json = JsonSerializer.Serialize(animeRanking, _options);
        _output.WriteLine(json);

        Assert.NotNull(animeRanking);
    }
    
    [Fact]
    public async Task AnimeSeasonTest()
    {
        var animeSeason = await _client.GetAnimeSeasonAsync(2024, "summer", "anime_score", 10);

        var json = JsonSerializer.Serialize(animeSeason, _options);
        _output.WriteLine(json);

        Assert.NotNull(animeSeason);
    }
}