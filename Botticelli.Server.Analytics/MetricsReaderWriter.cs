using Botticelli.Analytics.Shared.Metrics;
using Botticelli.Analytics.Shared.Requests;
using Botticelli.Analytics.Shared.Responses;
using Botticelli.Server.Analytics.Cache;
using Botticelli.Server.Analytics.Models;
using Botticelli.Server.Analytics.Utils;

namespace Botticelli.Server.Analytics;

public class MetricsReaderWriter
{
    private readonly MetricsContext _context;
    private readonly ICache _cache;
    public MetricsReaderWriter(MetricsContext context, ICache cache)
    {
        _context = context;
        _cache = cache;
    }

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

        IAsyncEnumerable<(DateTime dt1, DateTime dt2, int count)> result = new AsyncEnumerable<(DateTime dt1, DateTime dt2, int count)>();

        foreach (var frame in frames.ToBlockingEnumerable())
        {
            var cached = _cache.Get(new GetMetricsForIntervalsRequest()
            {
                From = frame.dt1,
                To = frame.dt2,
                Name = name
            });

            if (cached == null || cached.Count <= 0)
            {
                cached = new GetMetricsResponse()
                {
                    From = frame.dt1,
                    To = frame.dt2,
                    Count = _context.MetricModels.Count(mm => mm.BotId == botId &&
                                                              mm.Name == name &&
                                                              mm.Timestamp >= frame.dt1.ToUniversalTime() &&
                                                              mm.Timestamp <= frame.dt2.ToUniversalTime())
                };
            }

            if (cached.Count > 0)
                yield cached;
        }

        return frames.Select(f => (f.dt1, f.dt2, )))
            .Where(f => f.Item3 > 0);
    }
}