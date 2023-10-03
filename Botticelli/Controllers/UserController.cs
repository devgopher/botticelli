using Botticelli.Server.Data.Entities.Auth;
using Botticelli.Server.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Botticelli.Server.Controllers
{
    /// <summary>
    ///     Admin controller getting/adding/removing bots
    /// </summary>
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("user")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IConfirmationService _confirmationService;
        private readonly IAdminAuthService _adminAuthService;
        
        public UserController(IUserService userService, IConfirmationService confirmationService, IAdminAuthService adminAuthService)
        {
            _userService = userService;
            _confirmationService = confirmationService;
            _adminAuthService = adminAuthService;
        }

        [HttpPost()]
        [Authorize("Bearer", "")]
        public async Task<IActionResult> AddUser(UserAddRequest request, CancellationToken token)
        {
            try
            {
                await _userService.AddAsync(request, token);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }


        [HttpGet("[action]")]
        public async Task<ActionResult<UserGetResponse>> GetCurrentUser(CancellationToken token)
        {
            var user = GetCurrentUserName();
            var request = new UserGetRequest()
            {
                UserName = user
            };

            return await GetUser(request, token);
        }

        private string GetCurrentUserName() => HttpContext.User.Claims.FirstOrDefault(c => c.Type == "applicationUserName")?.Value;

        [HttpGet()]
        public async Task<ActionResult<UserGetResponse>> GetUser(UserGetRequest request, CancellationToken token)
        {
            try
            {
                return new ActionResult<UserGetResponse>(await _userService.GetAsync(request, token)); ;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateCurrentUser(UserUpdateRequest request, CancellationToken token)
        {
            var user = GetCurrentUserName();
            request.UserName = user;
            
            return await UpdateUser(request, token);
        }

        [HttpPut()]
        public async Task<IActionResult> UpdateUser(UserUpdateRequest request, CancellationToken token)
        {
            try
            {
                await _userService.UpdateAsync(request, token);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpDelete()]
        public async Task<IActionResult> DeleteUser(UserDeleteRequest request , CancellationToken token)
        {
            try
            {
                var user = GetCurrentUserName();
                if (request.UserName == user)
                    return BadRequest("You can't delete yourself!");

                await _userService.DeleteAsync(request, token);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail([FromQuery]ConfirmEmailRequest request, CancellationToken token)
        {
            try
            {
                var user = GetCurrentUserName();

                await _userService.ConfirmCodeAsync(request.Email, request.Token, token);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
