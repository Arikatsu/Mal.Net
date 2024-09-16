namespace Mal.Net.Utils;

internal class ApiUrl
{
    private readonly string _endpoint = "https://api.myanimelist.net/v2";
    
    private readonly string _path;
    private readonly List<string> _params;

    internal ApiUrl(string path, bool forAuth = false)
    {
        _path = path;
        _params = new List<string>();
        
        if (forAuth)
        {
            _endpoint = "https://myanimelist.net/v1";
        }
    }
    
    internal ApiUrl(string path, object parameters, bool forAuth = false)
    {
        _path = path;
        _params = parameters.GetType()
            .GetProperties()
            .Select(p => $"{p.Name}={p.GetValue(parameters)}")
            .ToList();
        
        if (forAuth)
        {
            _endpoint = "https://myanimelist.net/v1";
        }
    }
    
    internal string GetUrl()
    {
        return _params.Count == 0
            ? $"{_endpoint}/{_path}"
            : $"{_endpoint}/{_path}?{string.Join("&", _params)}";
    }

    internal string GetUrlWithoutParams()
    {
        return $"{_endpoint}/{_path}";
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
    
    internal ApiUrl AddParamIf(string key, int? value, bool condition = true)
    {
        if (condition && !value.HasValue)
        {
            _params.Add($"{key}={value}");
        }
        return this;
    }
}