﻿using Botticelli.Server.Data.Entities.Auth;
using Botticelli.Server.Services.Auth;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PasswordGenerator;

namespace Botticelli.Server.Controllers;

/// <summary>
///     Admin controller getting/adding/removing bots
/// </summary>
[ApiController]
[Authorize(AuthenticationSchemes = "Bearer")]
[Route("user")]
public class UserController : Controller
{
    private readonly IMapper _mapper;

    private readonly IPassword _password = new Password(true, true,
        true, false, 12);

    private readonly IPasswordSender _passwordSender;
    private readonly IUserService _userService;

    public UserController(IUserService userService, IConfirmationService confirmationService,
        IAdminAuthService adminAuthService, IMapper mapper, IPasswordSender passwordSender)
    {
        _userService = userService;
        _mapper = mapper;
        _passwordSender = passwordSender;
    }

    [HttpGet("[action]")]
    [AllowAnonymous]
    public async Task<ObjectResult> HasUsersAsync(CancellationToken token) => Ok(await _userService.HasUsers(token));

    [HttpPost("[action]")]
    [AllowAnonymous]
    public async Task<IActionResult> AddDefaultUserAsync(DefaultUserAddRequest request, CancellationToken token)
    {
        try
        {
            var password = _password.Next();
            var mapped = _mapper.Map<UserAddRequest>(request);
            mapped.Password = password;

            if (await _userService.CheckAndAddAsync(mapped, token))
                await _passwordSender.SendPassword(request.Email, password, token);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return Ok();
    }


    [HttpPost]
    public async Task<IActionResult> AddUserAsync(UserAddRequest request, CancellationToken token)
    {
        try
        {
            await _userService.AddAsync(request, true, token);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return Ok();
    }


    [HttpGet("[action]")]
    public async Task<ActionResult<UserGetResponse>> GetCurrentUserAsync(CancellationToken token)
    {
        var user = GetCurrentUserName();
        var request = new UserGetRequest
        {
            UserName = user
        };

        return await GetUserAsync(request, token);
    }

    private string GetCurrentUserName() =>
        HttpContext.User.Claims.FirstOrDefault(c => c.Type == "applicationUserName")?.Value;

    [HttpGet]
    public async Task<ActionResult<UserGetResponse>> GetUserAsync(UserGetRequest request, CancellationToken token)
    {
        try
        {
            return new ActionResult<UserGetResponse>(await _userService.GetAsync(request, token));
            ;
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("[action]")]
    public async Task<IActionResult> UpdateCurrentUserAsync(UserUpdateRequest request, CancellationToken token)
    {
        var user = GetCurrentUserName();
        request.UserName = user;

        return await UpdateUserAsync(request, token);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateUserAsync(UserUpdateRequest request, CancellationToken token)
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

    [HttpDelete]
    public async Task<IActionResult> DeleteUserAsync(UserDeleteRequest request, CancellationToken token)
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
    public async Task<IActionResult> ConfirmEmailAsync([FromQuery] ConfirmEmailRequest request, CancellationToken token)
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