using Microsoft.AspNetCore.Mvc;

namespace Botticelli.Server.Analytics.Controllers;

/// <summary>
///     Metrics receiver controller
/// </summary>
[ApiController]
[Route("metrics/getter")]
public class MetricsGetter : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}