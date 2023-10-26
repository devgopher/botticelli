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
[Route("/v1/auth")]
public class AuthController
{
    private readonly IAdminAuthService _adminAuthService;

    public AuthController(IAdminAuthService adminAuthService) => _adminAuthService = adminAuthService;

    [AllowAnonymous]
    [HttpPost("[action]")]
    public IActionResult GetToken(UserLoginRequest request)
        => new OkObjectResult(_adminAuthService.GenerateToken(request));

    [AllowAnonymous]
    [HttpPost("[action]")]
    [Obsolete($"Use {nameof(UserController)}.{nameof(UserController.AddUserAsync)}()!")]
    public async Task Register(UserAddRequest request)
        => await _adminAuthService.RegisterAsync(request);

    [AllowAnonymous]
    [HttpGet("[action]")]
    [Obsolete($"Use {nameof(UserController)}.{nameof(UserController.HasUsersAsync)}()!")]
    public async Task<bool> HasUsersAsync()
        => await _adminAuthService.HasUsersAsync();
}