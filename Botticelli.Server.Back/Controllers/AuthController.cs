using Botticelli.Server.Back.Services.Auth;
using Botticelli.Server.Data.Entities.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Botticelli.Server.Back.Controllers;

/// <summary>
///     Login controller for login/logoff/registration and access checking functions
/// </summary>
[ApiController]
[AllowAnonymous]
[Route("/v1/auth")]
public class AuthController(IAdminAuthService adminAuthService)
{
    private readonly IAdminAuthService _adminAuthService = adminAuthService;

    /// <summary>
    /// Gets auth token for a user
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost("[action]")]
    public IActionResult GetToken(UserLoginRequest request)
    {
        return new OkObjectResult(_adminAuthService.GenerateToken(request));
    }
}