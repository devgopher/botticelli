using System.Collections.Concurrent;
using Botticelli.Analytics.Shared.Requests;
using Botticelli.Analytics.Shared.Responses;
using Botticelli.Server.Analytics.Models;
using Botticelli.Server.Analytics.Utils;
using Polly;

namespace Botticelli.Server.Analytics.Cache;

/// <summary>
///     Local-based memory cache with auto-cleaning
/// </summary>
public class LocalCacheAccessor : ICacheAccessor
{
    private static readonly ConcurrentDictionary<string, MetricModel> _memoryCache = new();
    private static long _maxCacheSize = 1024768;
    private static Task<PolicyResult<bool>> _cacheCleaningTask;
    private static CancellationTokenSource _cancellationTokenSource = new();
    private static CancellationToken _ct = _cancellationTokenSource.Token;

    public LocalCacheAccessor(long maxCacheSize)
    {
        _maxCacheSize = maxCacheSize;
        InitCacheCleaning();
    }


    public int ReadCount(Func<MetricModel, bool> func)
        => _memoryCache
            .Values
            .Where(func)
            .Count();

    public GetMetricsIntervalsResponse GetForIntervals(GetMetricsForIntervalsRequest request)
    {
        var frames = DateTimeUtils.GetRange(request.From,
            request.To,
            TimeSpan.FromSeconds(request.Interval));

        var results = frames.Select(f => new GetMetricsResponse
        {
            From = f.dt1,
            To = f.dt2,
            Count = _memoryCache.Count(mm => mm.Value.BotId == request.BotId &&
                                             mm.Value.Name == request.Name &&
                                             mm.Value.Timestamp >= f.dt1.ToUniversalTime() &&
                                             mm.Value.Timestamp <= f.dt2.ToUniversalTime())
        }).Where(f => f.Count > 0);

        return new GetMetricsIntervalsResponse
        {
            From = request.From,
            To = request.To,
            MetricsForIntervals = results
        };
    }

    public void Set(MetricModel request)
    {
        _memoryCache[request.Id] = request;
    }

    public void Clear(DateTime until)
    {
        var toRemove = _memoryCache.Where(c => c.Value.Timestamp < until)
            .Select(kvp => kvp.Key).ToArray();

        foreach (var c in toRemove)
            _memoryCache.TryRemove(c, out var _);
    }

    public void Remove(MetricModel request)
    {
        _memoryCache.TryRemove(request.Id, out var _);
    }

    private static void InitCacheCleaning()
    {
        if (_cacheCleaningTask != null)
            return;

        _cacheCleaningTask = Policy.HandleResult<bool>(r => true)
            .WaitAndRetryForeverAsync(_ => TimeSpan.FromMinutes(1))
            .ExecuteAndCaptureAsync(async ct =>
            {
                var size = _memoryCache.Count;

                while (size >= _maxCacheSize)
                {
                    if (_ct.CanBeCanceled && _ct.IsCancellationRequested)
                        break;

                    var oldest = _memoryCache
                        .Values
                        .OrderBy(c => c.Timestamp)
                        .FirstOrDefault();

                    if (oldest == null)
                        break;

                    _memoryCache.TryRemove(oldest.Id, out var _);

                    size = _memoryCache.Count;
                }

                return true;
            }, _ct);
    }

    private static void RefreshCancellationToken()
    {
        _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(new CancellationToken());
        _ct = _cancellationTokenSource.Token;
    }

    public GetMetricsResponse Get(GetMetricsForIntervalsRequest request) =>
        new()
        {
            From = request.From,
            To = request.To,
            Count = _memoryCache.Count(mm => mm.Value.BotId == request.BotId &&
                                             mm.Value.Name == request.Name &&
                                             mm.Value.Timestamp >= request.From.ToUniversalTime() &&
                                             mm.Value.Timestamp <= request.To.ToUniversalTime())
        };
}