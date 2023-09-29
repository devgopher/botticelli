using Botticelli.Analytics.Shared.Metrics;
using Botticelli.Server.Analytics.Models;
using Botticelli.Server.Analytics.Utils;

namespace Botticelli.Server.Analytics;

public class MetricsReaderWriter
{
    private readonly MetricsContext _context;
    public MetricsReaderWriter(MetricsContext context) => _context = context;

    public async Task WriteAsync(MetricObject input, CancellationToken token)
    {
        await _context.MetricModels.AddAsync(new MetricModel
        {
            Id = input.Id,
            Timestamp = input.Timestamp,
            BotId = input.BotId,
            Name = input.Name
        }, token);

        await _context.SaveChangesAsync(token);
    }
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