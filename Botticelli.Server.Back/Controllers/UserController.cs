using Botticelli.Server.Back.Services.Auth;
using Botticelli.Server.Back.Settings;
using Botticelli.Server.Data.Entities.Auth;
using Botticelli.Shared.Utils;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PasswordGenerator;

namespace Botticelli.Server.Back.Controllers;

/// <summary>
///     Controller for user authorization on admin part
/// </summary>
[ApiController]
[Authorize(AuthenticationSchemes = "Bearer")]
[Route("/v1/user")]
public class UserController(IUserService userService, IMapper mapper, IPasswordSender passwordSender, ServerSettings settings) : Controller
{
    private readonly IPassword _password = new Password(true,
                                                        true,
                                                        true,
                                                        false,
                                                        Random.Shared.Next(settings.PasswordMinLength, 
                                                                           settings.PasswordMaxLength));

    /// <summary>
    ///     Does system contain any users?
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpGet("[action]")]
    [AllowAnonymous]
    public async Task<ObjectResult> HasUsersAsync(CancellationToken token)
    {
        return Ok(await userService.HasUsers(token));
    }

    /// <summary>
    ///     Gets registration settings for admin UI
    /// </summary>
    [HttpGet("[action]")]
    [AllowAnonymous]
    public ActionResult<RegistrationSettingsResponse> GetRegistrationSettings()
    {
        return Ok(new RegistrationSettingsResponse
        {
            PasswordDeliveryMode = settings.PasswordDeliveryMode
        });
    }

    /// <summary>
    ///     Adds a default user
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpPost("[action]")]
    [AllowAnonymous]
    public async Task<IActionResult> AddDefaultUserAsync(DefaultUserAddRequest request, CancellationToken token)
    {
        try
        {
            request.NotNull();
            request.UserName.NotNull();
            request.Email.NotNull();

            var password = _password.Next();
            var mapped = mapper.Map<UserAddRequest>(request);
            mapped.Password = password;

            if (await userService.CheckAndAddAsync(mapped, token))
            {
                if (settings.PasswordDeliveryMode == PasswordDeliveryMode.Email)
                    await passwordSender.SendPassword(request.Email!, password, token);

                return Ok(new DefaultUserAddResponse
                {
                    Password = settings.PasswordDeliveryMode == PasswordDeliveryMode.RegistrationWindow
                        ? password
                        : null
                });
            }

            return BadRequest("User already exists or registration failed.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    ///     Regenerates passsword
    /// </summary>
    /// <param name="passwordRequest"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpPost("[action]")]
    [AllowAnonymous]
    public async Task<IActionResult> RegeneratePasswordAsync(RegeneratePasswordRequest passwordRequest,
                                                             CancellationToken token)
    {
        try
        {
            passwordRequest.NotNull();
            passwordRequest.UserName.NotNull();
            passwordRequest.Email.NotNull();

            var mapped = mapper.Map<UserUpdateRequest>(passwordRequest);
            mapped.Password = _password.Next();

            await userService.UpdateAsync(mapped, token);
            await passwordSender.SendPassword(passwordRequest.Email!, mapped.Password, token);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return Ok();
    }

    /// <summary>
    ///     Adds a new user
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> AddUserAsync(UserAddRequest request, CancellationToken token)
    {
        try
        {
            await userService.AddAsync(request, true, token);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return Ok();
    }


    /// <summary>
    ///     Gets an authorized current user
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
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
    
    private string GetCurrentUserName()
    {
        return HttpContext.User.Claims.FirstOrDefault(c => c.Type == "applicationUserName")?.Value ??
               throw new NullReferenceException();
    }

    /// <summary>
    ///     Gets an information about a user
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<UserGetResponse>> GetUserAsync(UserGetRequest request, CancellationToken token)
    {
        try
        {
            return new ActionResult<UserGetResponse>(await userService.GetAsync(request, token));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    ///     Updates a current user
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpPut("[action]")]
    public async Task<IActionResult> UpdateCurrentUserAsync(UserUpdateRequest request, CancellationToken token)
    {
        var user = GetCurrentUserName();
        request.UserName = user;

        return await UpdateUserAsync(request, token);
    }

    /// <summary>
    ///     Updates a given user
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<IActionResult> UpdateUserAsync(UserUpdateRequest request, CancellationToken token)
    {
        try
        {
            await userService.UpdateAsync(request, token);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    ///     Deletes a user
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<IActionResult> DeleteUserAsync(UserDeleteRequest request, CancellationToken token)
    {
        try
        {
            var user = GetCurrentUserName();

            if (request.UserName == user) return BadRequest("You can't delete yourself!");

            await userService.DeleteAsync(request, token);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    ///     Email confirmation method
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpGet("[action]")]
    [AllowAnonymous]
    public async Task<IActionResult> ConfirmEmailAsync([FromQuery] ConfirmEmailRequest request, CancellationToken token)
    {
        try
        {
            request.NotNull();
            request.Email.NotNull();
            request.Token.NotNull();

            await userService.ConfirmCodeAsync(request.Email!, request.Token!, token);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}