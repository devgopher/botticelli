using Botticelli.Analytics.Shared.Requests;
using Botticelli.Analytics.Shared.Responses;
using Botticelli.Server.Analytics.Models;
using Botticelli.Server.Analytics.Utils;
using System.Collections.Concurrent;

namespace Botticelli.Server.Analytics.Cache
{
    public class Cache : ICache
    {
        private readonly ConcurrentDictionary<CacheRequest, string> _memoryCache = new();

        public GetMetricsResponse Get(GetMetricsForIntervalsRequest request)
        {
            return new GetMetricsResponse()
            {
                From = request.From,
                To = request.To,
                Count = _memoryCache.Count(mm => mm.Key.BotId == request.BotId &&
                                                 mm.Value == request.Name &&
                                                 mm.Key.Timestamp >= request.From.ToUniversalTime() &&
                                                 mm.Key.Timestamp <= request.To.ToUniversalTime())
            };
        }

        public GetMetricsIntervalsResponse GetForIntervals(GetMetricsForIntervalsRequest request)
        {
            var frames = DateTimeUtils.GetRange(request.From,
                                                request.To,
                                                TimeSpan.FromSeconds(request.Interval));

            var results = frames.Select(f => new GetMetricsResponse()
            {
                From = f.dt1,
                To = f.dt2,
                Count = _memoryCache.Count(mm => mm.Key.BotId == request.BotId &&
                                                 mm.Value == request.Name &&
                                                 mm.Key.Timestamp >= f.dt1.ToUniversalTime() &&
                                                 mm.Key.Timestamp <= f.dt2.ToUniversalTime())
            }).Where(f => f.Count > 0);

            return new GetMetricsIntervalsResponse()
            {
                From = request.From,
                To = request.To,
                MetricsForIntervals = results
            };
        }

        public void Set(MetricModel request)
        {
            _memoryCache.TryAdd(new CacheRequest
            {
                BotId = request.BotId,
                Timestamp = request.Timestamp,
            }, request.Name);
        }

        public void Clear(DateTime until)
        {
            var toRemove = _memoryCache.Where(c => c.Key.Timestamp < until).ToArray();

            foreach (var c in toRemove)
                _memoryCache.TryRemove(c);
        }
    }
}
