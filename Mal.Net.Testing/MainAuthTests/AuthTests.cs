using System.Net;
using System.Diagnostics;
using Xunit.Abstractions;

namespace Mal.Net.Testing.MainAuthTests;

public class AuthTests : TestsBase
{
    public AuthTests(ITestOutputHelper output) : base(output) { }
    
    [Fact]
    public async Task AuthFlowTest()
    {
        var authUrl = Client.GenerateAuthorizationUrl(out var state, out var codeVerfier);
        
        Assert.NotNull(authUrl);
        Assert.NotNull(state);
        Assert.NotNull(codeVerfier);
        
        var tcs = new TaskCompletionSource<string>();
        
        var server = new HttpListener();
        server.Prefixes.Add(Config.RedirectUri);
        server.Start();
        
        Process.Start(new ProcessStartInfo(authUrl) { UseShellExecute = true });

        server.BeginGetContext(result =>
        {
            var context = server.EndGetContext(result);
            var request = context.Request;
            var code = request.QueryString["code"] ?? "error";
            
            tcs.SetResult(code);

        }, null);
        
        var code = await tcs.Task;
        
        server.Stop();
        
        Assert.NotEqual("error", code);
        
        var user = await Client.AuthenticateUserAsync(code, codeVerfier);
        
        Assert.NotNull(user.AccessToken);
        Assert.NotNull(user.RefreshToken);
        
        ConfigManager.Update(config =>
        {
            config.AccessToken = user.AccessToken;
            config.RefreshToken = user.RefreshToken;
        });
    }
    
    [Fact]
    public async Task RefreshTokenTest()
    {
        var previousAccessToken = User.AccessToken;
        var previousRefreshToken = User.RefreshToken;
        
        await User.RefreshAccessTokenAsync(true);
        
        Assert.NotEqual(previousAccessToken, User.AccessToken);
        Assert.NotEqual(previousRefreshToken, User.RefreshToken);
    }
}


