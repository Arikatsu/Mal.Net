using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;

namespace Mal.Net.Testing;

public class MalClientTests
{
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

        var json = JsonSerializer.Serialize(animeList, new JsonSerializerOptions { WriteIndented = true });
        _output.WriteLine(json);

        Assert.NotNull(animeList);
    }
    
    [Fact]
    public async Task AnimeDetailsTest()
    {
        IEnumerable<string> fields = new List<string> { AnimeFields.Rating, AnimeFields.StartDate };

        var animeDetails = await _client.GetAnimeDetailsAsync(33161, fields);

        var json = JsonSerializer.Serialize(animeDetails, new JsonSerializerOptions { WriteIndented = true });
        _output.WriteLine(json);

        Assert.NotNull(animeDetails);
    }
}