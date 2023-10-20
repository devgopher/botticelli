using Botticelli.Analytics.Shared.Requests;
using Botticelli.Server.Analytics.Services;
using Microsoft.AspNetCore.Mvc;

namespace Botticelli.Server.Analytics.Controllers;

/// <summary>
///     MetricsOutputController receiver controller
/// </summary>
[ApiController]
[Route("metrics/receiver")]
public class MetricsInputController : Controller
{
    private readonly IMetricsInputService _service;

    public MetricsInputController(IMetricsInputService service) => _service = service;

    [HttpGet("[action]")]
    public async Task<IActionResult> ReceiveMetric([FromQuery] PushMetricRequest request, CancellationToken token)
    {
        try
        {
            await _service.PushMetricAsync(request, token);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return Ok();
    }
}