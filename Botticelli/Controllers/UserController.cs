using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Botticelli.Server.Controllers
{
    /// <summary>
    ///     Admin controller getting/adding/removing bots
    /// </summary>
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("admin")]
    public class UserController : Controller
    {
        [HttpPost()]
        public IActionResult AddUser()
        {
            return NotFound();
        }

        [HttpGet()]
        public IActionResult GetUser()
        {
            return NotFound();
        }

        [HttpPut()]
        public IActionResult UpdateUser()
        {
            return NotFound();
        }
        
        [HttpDelete()]
        public IActionResult DeleteUser()
        {
            return NotFound();
        }
    }
}
