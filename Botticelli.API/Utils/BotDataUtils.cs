using Botticelli.Shared.Utils;

namespace Botticelli.BotBase.Utils;

public static class BotDataUtils
{
    private const string SubDir = "Data";

    public static string? GetPath()
    {
        return Path.Combine(SubDir, "botId");
    }

    public static string? GetBotId()
    {
        string uid;

        if (!File.Exists(GetPath()))
        {
            Directory.CreateDirectory(SubDir);
            uid = Uid.GenerateShortUid();
            File.WriteAllText(GetPath()!, uid);
        }
        else
        {
            uid = File.ReadAllText(GetPath()!);
        }

        return uid;
    }
}