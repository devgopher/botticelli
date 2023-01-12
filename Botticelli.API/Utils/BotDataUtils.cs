using Botticelli.Shared.Utils;

namespace Botticelli.BotBase.Utils
{
    public static class BotDataUtils
    {
        public static string? GetPath()
            => Path.Combine("Data", "botId");

        public static string? GetBotId()
        {
            string uid;
            if (!File.Exists(GetPath()))
            {
                uid = Uid.GenerateShortUid();
                File.WriteAllText(GetPath()!, uid);
            }
            else
                uid = File.ReadAllText(GetPath()!);

            return uid;
        }
    }
}
