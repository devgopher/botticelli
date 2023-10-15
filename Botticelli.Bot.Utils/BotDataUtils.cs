using System.IO;
using Botticelli.Shared.Utils;

namespace Botticelli.BotBase.Utils
{

    public static class BotDataUtils
    {
        private const string SubDir = "Data";
        private static string _botId = null;

        public static string? GetPath() => Path.Combine(SubDir, "botId");

        public static string? GetBotId()
        {
            if (!File.Exists(GetPath()))
            {
                Directory.CreateDirectory(SubDir);
                _botId = BotIdUtils.GenerateShortBotId();
                File.WriteAllText(GetPath()!, _botId);
            }
            else
            {
                _botId ??= File.ReadAllText(GetPath()!);
            }

            return _botId;
        }
    }
}