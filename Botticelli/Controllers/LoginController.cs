using Botticelli.Server.Data.Entities.Auth;
using Botticelli.Server.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Botticelli.Server.Controllers;

/// <summary>
///     Login controller for login/logoff/registration and access checking functions
/// </summary>
[ApiController]
[AllowAnonymous]
[Route("login")]
public class LoginController
{
    private readonly AdminAuthService _adminAuthService;

    public LoginController(AdminAuthService adminAuthService) => _adminAuthService = adminAuthService;

    [AllowAnonymous]
    [HttpPost("[action]")]
    public IActionResult GetToken(UserLoginPost request)
        => new OkObjectResult(_adminAuthService.GenerateToken(request));

    [AllowAnonymous]
    [HttpPost("[action]")]
    public async Task Register(UserRegisterPost request)
        => await _adminAuthService.RegisterAsync(request);
}