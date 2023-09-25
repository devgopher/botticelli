using System.Text.Json;
using Botticelli.Analytics.Shared.Metrics;
using Botticelli.Server.Analytics.Models;

namespace Botticelli.Server.Analytics
{
    public class MetricsWriter
    {
        private readonly MetricsContext _context;
        public MetricsWriter(MetricsContext context) => _context = context;

        public void Write<T>(MetricObject<T> input)
        {
            _context.MetricModels.Add(new MetricModel()
            {
                Id = input.Id,
                Timestamp = input.Timestamp,
                Value = JsonSerializer.Serialize(input.Value)
            });

            _context.SaveChanges();
        }
    }
}
