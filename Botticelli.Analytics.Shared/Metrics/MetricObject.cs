namespace Botticelli.Analytics.Shared.Metrics
{
    public class MetricObject<TObject>
    {
        public MetricObject(TObject args)
        {
            Value = args;
            Timestamp = DateTime.Now;
        }

        public long Id => DateTime.UtcNow.Ticks;

        public DateTime Timestamp { get; set; }
        public TObject Value { get; set; }
    }
}
