namespace Mal.Net.Utils;

internal class ApiUrl
{
    private const string Endpoint = "https://api.myanimelist.net/v2";
    
    private readonly string _path;
    private readonly List<string> _params;

    internal ApiUrl(string path)
    {
        _path = path;
        _params = new List<string>();
    }
    
    internal ApiUrl(string path, object parameters)
    {
        _path = path;
        _params = parameters.GetType()
            .GetProperties()
            .Select(p => $"{p.Name}={p.GetValue(parameters)}")
            .ToList();
    }
    
    internal string GetUrl()
    {
        return _params.Count == 0
            ? $"{Endpoint}/{_path}"
            : $"{Endpoint}/{_path}?{string.Join("&", _params)}";
    }

    internal string GetUrlWithoutParams()
    {
        return $"{Endpoint}/{_path}";
    }

    internal ApiUrl AddParam(string key, string value)
    {
        _params.Add($"{key}={value}");
        return this;
    }
    
    internal ApiUrl AddParamIf(string key, string? value, bool condition = true)
    {
        if (condition && !string.IsNullOrEmpty(value))
        {
            _params.Add($"{key}={value}");
        }
        return this;
    }
}