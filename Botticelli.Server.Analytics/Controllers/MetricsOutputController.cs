using Botticelli.Analytics.Shared.Requests;
using Botticelli.Analytics.Shared.Responses;
using Botticelli.Server.Analytics.Services;
using Microsoft.AspNetCore.Mvc;

namespace Botticelli.Server.Analytics.Controllers;

/// <summary>
///     MetricsOutputController receiver controller
/// </summary>
[ApiController]
[Route("metrics/getter")]
public class MetricsOutputController : Controller
{
    private readonly IMetricsOutputService _service;

    public MetricsOutputController(IMetricsOutputService service) => _service = service;

    [HttpGet("[action]")]
    public async Task<GetMetricsResponse> GetMetrics([FromQuery] GetMetricsRequest request, CancellationToken token) 
        => await _service.GetMetricsAsync(request, token);

    [HttpGet("[action]")]
    public async Task<GetMetricsIntervalsResponse> GetMetricsForInterval([FromQuery] GetMetricsForIntervalsRequest request, CancellationToken token)
        => await _service.GetMetricsForIntervalAsync(request, token);
}