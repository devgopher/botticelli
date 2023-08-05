using System.Security.Cryptography;
using System.Text;

namespace Botticelli.Shared.Utils;

/// <summary>
///     Hash generation utils
/// </summary>
public static class HashUtils
{
    private static readonly SHA512 Encryptor = SHA512.Create();

    public static string GetHash(string input, string? salt)
    {
        var hashed = Encryptor.ComputeHash(Encoding.UTF8.GetBytes(input + salt));

        return Convert.ToBase64String(hashed);
    }
}