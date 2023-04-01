using System.Security.Cryptography;
using System.Text;

namespace Botticelli.Shared.Utils;

/// <summary>
///     Hash generation utils
/// </summary>
public static class HashUtils
{
    private static readonly SHA512 _encryptor = SHA512.Create();

    public static string GetHash(string input, string? salt)
    {
        var hashed = _encryptor.ComputeHash(Encoding.UTF8.GetBytes(input + salt));

        return Convert.ToBase64String(hashed);
    }
}