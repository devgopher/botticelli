using System.Collections.Immutable;

namespace Botticelli.Analytics.Shared.Metrics
{
    /// <summary>
    /// Names for typical metrics
    /// </summary>
    public static class MetricNames
    {
        public const string MessageReceived = "MessageReceived";
        public const string MessageSent = "MessageSent";
        public const string MessageRemoved = "MessageRemoved";
        public const string NewSubscription = "NewSubscription";
        public const string UnSubscription = "UnSubscription";
        public const string BotStarted = "BotStarted";
        public const string BotStopped = "BotStopped";

        public static string[] Names =
                new[]
                {
                    MessageReceived,
                    MessageSent,
                    MessageRemoved,
                    BotStarted,
                    BotStopped,
                    NewSubscription,
                    UnSubscription
                };
    }
}
