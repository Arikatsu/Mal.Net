using System.Security.Cryptography;

namespace Mal.Net.Utils;

internal static class AuthHelper
{
    internal static string GenerateState()
    {
        var random = RandomNumberGenerator.Create();
        var data = new byte[16];
        
        random.GetBytes(data);
        return Convert.ToBase64String(data).Replace("/", "").Replace("+", "").Replace("=", "");
    }
    
    internal static string GenerateCodeVerifier()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-._~";
        var random = new Random();
        var codeVerifier = new char[128];

        for (var i = 0; i < codeVerifier.Length; i++)
        {
            codeVerifier[i] = chars[random.Next(chars.Length)];
        }

        return new string(codeVerifier);
    }
}