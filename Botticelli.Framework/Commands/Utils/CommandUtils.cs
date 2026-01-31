using System.Text;
using System.Text.RegularExpressions;

namespace Botticelli.Framework.Commands.Utils;

public static class CommandUtils
{
    public static Regex SimpleCommandRegex => new("\\/([a-zA-Z0-9]*)$");
    public static Regex ArgsCommandRegex => new("\\/([a-zA-Z0-9]*) (.*)");

    public static string GetArguments(this string? body)
    {
        if (body is null) return string.Empty;

        var match = ArgsCommandRegex.Matches(body)
                                    .FirstOrDefault();

        if (match == null) 
            return string.Empty;

        var result = RemoveOddWhitespaces(match);

        return result.ToString();

    }

    private static StringBuilder RemoveOddWhitespaces(Match match)
    {
        var current = match.Groups[2].Value.Trim();
        var result = new StringBuilder();

        char prev = default;
        foreach (var chr in current)
        {
            if (prev != default)
            {
                if (Char.IsWhiteSpace(prev) && Char.IsWhiteSpace(chr))
                    continue;
            }
                
            result.Append(chr);
            prev = chr;
        }

        return result;
    }
}