using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;

namespace Mal.Net.Testing
{
    public class MalClientTests
    {
        private readonly IConfiguration _configuration;
        
        public MalClientTests(ITestOutputHelper output)
        {
            Console.SetOut(new LogConverter(output));

            var builder = new ConfigurationBuilder()
                .AddUserSecrets<MalClientTests>()
                .Build();

            _configuration = builder;
        }
        
        [Fact]
        public void AnimeListTest()
        {
            string? clientId = _configuration["Secrets:ClientId"];
            string? clientSecret = _configuration["Secrets:ClientSecret"];

            Assert.NotNull(clientId);
            Assert.NotNull(clientSecret);

            var client = new MalClient(clientId, clientSecret);

            var animeList = client.GetAnimeListAsync("oregairu", 10).Result;

            Console.WriteLine(animeList);

            client.Dispose();

            Assert.NotNull(animeList);
        }
    }
}