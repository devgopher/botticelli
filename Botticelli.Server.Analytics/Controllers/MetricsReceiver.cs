using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Botticelli.Server.Analytics.Controllers
{

    /// <summary>
    ///     Admin controller getting/adding/removing bots
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
}
