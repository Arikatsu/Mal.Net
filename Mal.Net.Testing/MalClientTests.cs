using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;

namespace Mal.Net.Testing;

public class MalClientTests
{
    private readonly IConfiguration _configuration;
    private readonly ITestOutputHelper _output;

    public MalClientTests(ITestOutputHelper output)
    {
        Console.SetOut(new LogConverter(output));

        var builder = new ConfigurationBuilder()
            .AddUserSecrets<MalClientTests>()
            .Build();

        _configuration = builder;
        _output = output;
    }

    [Fact]
    public void AnimeListTest()
    {
        var clientId = _configuration["Secrets:ClientId"];
        var clientSecret = _configuration["Secrets:ClientSecret"];

        Assert.NotNull(clientId);
        Assert.NotNull(clientSecret);

        var client = new MalClient(clientId, clientSecret);
        
        IEnumerable<string> fields = new List<string> { AnimeFields.Rating, AnimeFields.StartDate };
        
        var animeList = client.GetAnimeListAsync("oregairu", 1, 1, fields).Result;

        var json = JsonSerializer.Serialize(animeList, new JsonSerializerOptions { WriteIndented = true });
        _output.WriteLine(json);

        client.Dispose();

        Assert.NotNull(animeList);
    }
}