using System.Text.Json;
using Xunit.Abstractions;
using Mal.Net.Testing.Utils;

namespace Mal.Net.Testing;

public class TestsBase
{
    protected readonly JsonSerializerOptions Options = new() { WriteIndented = true };
    protected readonly ITestOutputHelper Output;
    
    protected readonly MalClient Client;
    protected readonly MalUser User;
    
    protected readonly ConfigManager ConfigManager;
    protected readonly Config Config;

    protected TestsBase(ITestOutputHelper output)
    {
        Console.SetOut(new LogConverter(output));
        Output = output;
        
        ConfigManager = new ConfigManager("../../../config.json");
        Config = ConfigManager.Get();
        
        Client = new MalClient(Config.ClientId, Config.ClientSecret);
        User = new MalUser(Config.AccessToken, Config.RefreshToken, "Bearer", 3600000, Config.ClientId,
            Config.ClientSecret);
    }
}