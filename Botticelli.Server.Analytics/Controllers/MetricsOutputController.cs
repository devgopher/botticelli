using Botticelli.Server.Analytics.Requests;
using Botticelli.Server.Analytics.Responses;
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

    [Route("[action]")]
    public GetMetricsResponse GetMetrics(GetMetricsRequest request) => _service.GetMetrics(request);

    [Route("[action]")]
    public GetMetricsIntervalsResponse GetMetricsForInterval(GetMetricsForIntervalsRequest request)
        => _service.GetMetricsForInterval(request);
}