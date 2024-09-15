using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;

namespace Mal.Net.Testing;

public class MalClientTestsBase
{
    protected readonly JsonSerializerOptions Options = new() { WriteIndented = true };
    protected readonly ITestOutputHelper Output;
    protected readonly MalClient Client;

    protected MalClientTestsBase(ITestOutputHelper output)
    {
        Console.SetOut(new LogConverter(output));

        var builder = new ConfigurationBuilder()
            .AddUserSecrets<MalClientTestsBase>()
            .Build();

        IConfiguration configuration = builder;
        
        Output = output;
        Client = new MalClient(configuration["Secrets:ClientId"]!, configuration["Secrets:ClientSecret"]!);
    }
}