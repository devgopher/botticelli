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
            Timestamp = input.Timestamp.ToUniversalTime(),
            BotId = input.BotId,
            Name = input.Name
        }, token);

        await _context.SaveChangesAsync(token);
    }

    public async Task<int> ReadCountAsync(Func<MetricModel, bool> func, CancellationToken token)
        => await _context.MetricModels
            .AsAsyncEnumerable()
            .Where(func)
            .CountAsync(token);

    public async Task<IEnumerable<string>> GetMetricNamesAsync(CancellationToken token)
        => _context.MetricModels.Select(x => x.Name).Distinct();

    public async Task<IAsyncEnumerable<(DateTime dt1, DateTime dt2, int count)>> ReadCountForFramesAsync(string name,
                                                                   string botId,
                                                                   DateTime from,
                                                                   DateTime to,
                                                                   TimeSpan frameLength,
                                                                   CancellationToken token)
    {
        var frames = DateTimeUtils.GetRange(from, to, frameLength, token);

        return frames.Select(f => (f.dt1, f.dt2,  _context.MetricModels
                                                         .Count(mm =>
                                                                        mm.BotId == botId &&
                                                                        mm.Name == name &&
                                                                        mm.Timestamp >= f.dt1.ToUniversalTime() &&
                                                                        mm.Timestamp <= f.dt2.ToUniversalTime())))
            .Where(f => f.Item3 > 0);
    }
}