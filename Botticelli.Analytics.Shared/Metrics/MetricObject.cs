namespace Botticelli.Analytics.Shared.Metrics
{
    public class MetricObject<TObject>
    {
        public MetricObject(TObject args, string botId)
        {
            Value = args;
            Timestamp = DateTime.Now;
            BotId = botId;
        }

        public string Id => Guid.NewGuid().ToString();
        public string BotId { get; set; }

        public DateTime Timestamp { get; set; }
        public TObject Value { get; set; }
    }

    public class MessageSentMetric
    {
        public string ChatId { get; set; }
        public string MessageId { get; set; }
    }

    public class MessageReceivedMetric
    {
        public string ChatId { get; set; }
        public string MessageId { get; set; }
    }

    public class MessageRemovedMetric
    {
        public string ChatId { get; set; }
        public string MessageId { get; set; }
    }

    public class CommandReceivedMetric
    {
        public string BotId { get; set; }
        public string ChatId { get; set; }
        public string MessageId { get; set; }
    }


    public class StartedBotMetric
    {
        public string BotId { get; set; }
    }

    public class StoppedBotMetric
    {
        public string BotId { get; set; }
    }

}
