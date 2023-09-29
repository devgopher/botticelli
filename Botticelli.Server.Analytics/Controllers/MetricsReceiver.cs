using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Botticelli.Server.Analytics.Controllers;

/// <summary>
///     MetricsOutputController receiver controller
/// </summary>
[ApiController]
[Route("metrics/receiver")]
public class MetricsReceiver : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}