using System.Text;

namespace Botticelli.Shared.Utils;

public static class StringUtils
{
    public static bool ContainsStrings(this string text, params string[] chunks)
    {
        foreach (var chunk in chunks)
            if (!text.Contains(chunk))
                return false;

        return true;
    }

    public static string ToSnakeCase(this string text)
    {
        if (text == null) throw new ArgumentNullException(nameof(text));

        if (text.Length < 2) return text;

        var sb = new StringBuilder();
        sb.Append(char.ToLowerInvariant(text[0]));

        for (var i = 1; i < text.Length; ++i)
        {
            var c = text[i];

            if (char.IsUpper(c))
            {
                sb.Append('_');
                sb.Append(char.ToLowerInvariant(c));
            }
            else
            {
                sb.Append(c);
            }
        }

        return sb.ToString();
    }
}