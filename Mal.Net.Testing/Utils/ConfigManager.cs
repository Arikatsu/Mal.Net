using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mal.Net.Testing.Utils;

public class ConfigManager
{
    private readonly string _configPath;
    private readonly Config _config;
    private readonly JsonSerializerOptions _options = new() { WriteIndented = true };
    
    public ConfigManager(string configPath)
    {
        _configPath = configPath;
        var json = File.ReadAllText(_configPath);
        _config = JsonSerializer.Deserialize<Config>(json) ?? throw new Exception("Failed to load config. Make sure the config file is correct.");
    }
    
    private void Save()
    {
        var json = JsonSerializer.Serialize(_config, _options);
        File.WriteAllText(_configPath, json, Encoding.UTF8);
    }
    
    public Config Get() => _config;

    public void Update(Action<Config> update)
    {
        update(_config);
        Save();
    }
}

public class Config
{
    [JsonPropertyName("client_id")]
    public string ClientId { get; set; } = string.Empty;
    
    [JsonPropertyName("client_secret")]
    public string ClientSecret { get; set; } = string.Empty;
    
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; } = string.Empty;
    
    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; set; } = string.Empty;
    
    [JsonPropertyName("redirect_uri")]
    public string RedirectUri { get; set; } = string.Empty;
}