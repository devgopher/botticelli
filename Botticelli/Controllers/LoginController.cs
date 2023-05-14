using Botticelli.Server.Data.Entities.Auth;
using Botticelli.Server.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Botticelli.Server.Controllers;

/// <summary>
/// Login controller for login/logoff/registration and access checking functions
/// </summary>
[ApiController]
[AllowAnonymous]
public class LoginController 
{
    private readonly AuthService _authService;

    public LoginController(AuthService authService) => _authService = authService;

    [AllowAnonymous]
    [HttpPost("[action]")]
    public IActionResult GetToken(UserLoginPost request)
        => new OkObjectResult(_authService.GenerateToken(request));

    [AllowAnonymous]
    [HttpPost("[action]")]
    public async Task Register(UserRegisterPost request)
        => await _authService.RegisterAsync(request);
}