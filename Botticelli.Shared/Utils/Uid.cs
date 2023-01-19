using System.Text.RegularExpressions;

namespace Botticelli.Shared.Utils;

public static class Uid
{
    public static string GenerateShortUid()
    {
        return Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", string.Empty);
    }
}