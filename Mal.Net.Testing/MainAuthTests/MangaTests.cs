using System.Text.Json;
using Xunit.Abstractions;

namespace Mal.Net.Testing.MainAuthTests;

public class MangaTests : MalClientTestsBase
{
    public MangaTests(ITestOutputHelper output) : base(output) { }
    
    [Fact]
    public async Task MangaListTest()
    {
        var mangaList = await Client.GetMangaListAsync("oregairu");
        
        var json = JsonSerializer.Serialize(mangaList, Options);
        Output.WriteLine(json);
        
        Assert.NotNull(mangaList);
    }
    
    [Fact]
    public async Task MangaRankingTest()
    {
        var mangaRanking = await Client.GetMangaRankingAsync("novels");
        
        var json = JsonSerializer.Serialize(mangaRanking, Options);
        Output.WriteLine(json);
        
        Assert.NotNull(mangaRanking);
    }
    
    [Fact]
    public async Task MangaDetailTest()
    {
        var mangaDetail = await Client.GetMangaDetailsAsync(40171);
        
        var json = JsonSerializer.Serialize(mangaDetail, Options);
        Output.WriteLine(json);
        
        Assert.NotNull(mangaDetail);
    }
}