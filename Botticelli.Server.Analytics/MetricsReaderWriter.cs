using System.Text.Json;
using Botticelli.Analytics.Shared.Metrics;
using Botticelli.Server.Analytics.Models;
using Botticelli.Server.Analytics.Utils;

namespace Botticelli.Server.Analytics;

public class MetricsReaderWriter
{
    private readonly MetricsContext _context;
    public MetricsReaderWriter(MetricsContext context) => _context = context;

    public async Task Write<T>(MetricObject<T> input)
    {
        _context.MetricModels.Add(new MetricModel
        {
            Id = input.Id,
            Timestamp = input.Timestamp,
            BotId = input.BotId,
            Value = JsonSerializer.Serialize(input.Value)
        });

        await _context.SaveChangesAsync();
    }

    public IEnumerable<MetricObject<T>> Read<T>(Func<MetricModel, bool> func)
        => _context.MetricModels
                   .Where(func)
                   .Select(x => new MetricObject<T>(JsonSerializer.Deserialize<T>(x.Value), x.BotId));

    public int ReadCount(Func<MetricModel, bool> func)
        => _context.MetricModels
                   .Where(func)
                   .Count();


    public IEnumerable<(DateTime dt1, DateTime dt2, int count)> ReadCountForFrames(string name,
                                                                   string botId,
                                                                   DateTime from,
                                                                   DateTime to,
                                                                   TimeSpan frameLength)
    {
        var frames = DateTimeUtils.GetRange(from, to, frameLength);

        return frames.Select(f => (f.dt1, f.dt2, _context.MetricModels
                                                         .Count(mm =>
                                                                        mm.BotId == botId &&
                                                                        mm.Name == name &&
                                                                        mm.Timestamp >= f.dt1 &&
                                                                        mm.Timestamp <= f.dt2)));
    }
}