using System.Text.Json;
using Xunit.Abstractions;

namespace Mal.Net.Testing.ClientAuthTests;

public class ForumTests : TestsBase
{
    public ForumTests(ITestOutputHelper output) : base(output) { }
    
    [Fact]
    public async Task ForumBoardsTest()
    {
        var forumBoards = await Client.GetForumBoardsAsync();
        
        var json = JsonSerializer.Serialize(forumBoards, Options);
        Output.WriteLine(json);
        
        Assert.NotNull(forumBoards);
    }
    
    [Fact]
    public async Task ForumTopicsDetailTest()
    {
        var forumTopics = await Client.GetForumTopicsDetailAsync(2179108);
        
        var json = JsonSerializer.Serialize(forumTopics, Options);
        Output.WriteLine(json);
        
        Assert.NotNull(forumTopics);
    }
    
    [Fact]
    public async Task ForumTopicsTest()
    {
        var forumTopics = await Client.GetForumTopicsAsync(new ForumTopicOptions
        {
            Limit = 10,
            Query = "oregairu"
        });
        
        var json = JsonSerializer.Serialize(forumTopics, Options);
        Output.WriteLine(json);
        
        Assert.NotNull(forumTopics);
    }
}