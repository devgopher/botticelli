using Botticelli.Server.Data;
using Botticelli.Server.Data.Entities.Auth;
using Botticelli.Server.Data.Exceptions;
using Botticelli.Shared.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Botticelli.Server.Services.Auth;

public class UserService : IUserService
{
    private readonly IConfiguration _config;
    private readonly IConfirmationService _confirmationService;
    private readonly BotInfoContext _context;
    private readonly ILogger<UserService> _logger;

    public UserService(IConfiguration config,
        BotInfoContext context,
        ILogger<UserService> logger,
        IConfirmationService confirmationService)
    {
        _config = config;
        _context = context;
        _logger = logger;
        _confirmationService = confirmationService;
    }

    public async Task<bool> HasUsers(CancellationToken token)
        => await _context.ApplicationUsers.AnyAsync(token);

    public async Task<bool> CheckAndAddAsync(UserAddRequest request, CancellationToken token)
    {
        try
        {
            if (await _context.ApplicationUsers.AnyAsync(token))
                return false;

            await AddAsync(request, false, token);

            var user = await _context.ApplicationUsers.FirstOrDefaultAsync(
                u => u.NormalizedEmail == GetNormalized(request.Email), token);
            user.EmailConfirmed = true;
            await _context.SaveChangesAsync(token);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(CheckAndAddAsync)}({request.UserName}) error: {ex.Message}", ex);

            throw;
        }
    }

    public async Task AddAsync(UserAddRequest request, bool needConfirmation, CancellationToken token)
    {
        try
        {
            _logger.LogInformation($"{nameof(AddAsync)}({request.UserName}) started...");

            if (_context.ApplicationUsers.AsQueryable()
                .Any(u => u.NormalizedUserName == GetNormalized(request.UserName) ||
                          u.Email == GetNormalized(request.Email)))
                throw new DataException($"User with name {request.UserName} and/or email {request.Email}" +
                                        " already exists!");

            var user = new IdentityUser
            {
                Id = Guid.NewGuid().ToString(),
                Email = request.Email,
                UserName = request.UserName,
                NormalizedUserName = GetNormalized(request.UserName),
                NormalizedEmail = GetNormalized(request.Email),
                PasswordHash = HashUtils.GetHash(request.Password, _config["Authorization:Salt"])
            };
            
            #if DEBUG
            if (request.Email == "test@test.com")
            {
                _logger.LogInformation("Test login password: {password}", request.Password);

                user.EmailConfirmed = true;

                needConfirmation = false;
            }
            #endif


            // Temporary - because now we assume, that we've only a single role - "admin"! 
            var appRole = new IdentityUserRole<string>
            {
                UserId = user.Id,
                RoleId = _context.ApplicationRoles.FirstOrDefault()?.Id ?? "-1"
            };

            await _context.ApplicationUsers.AddAsync(user, token);
            await _context.ApplicationUserRoles.AddAsync(appRole, token);

            await _context.SaveChangesAsync(token);

            if (needConfirmation)
            {
                _logger.LogInformation(
                    $"{nameof(AddAsync)}({request.UserName}) sending a confirmation email to {request.Email}...");
                await _confirmationService.SendConfirmationCode(user, token);
            }

            _logger.LogInformation($"{nameof(AddAsync)}({request.UserName}) finished...");
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(AddAsync)}({request.UserName}) error: {ex.Message}", ex);

            throw;
        }
    }

    public async Task UpdateAsync(UserUpdateRequest request, CancellationToken token)
    {
        try
        {
            _logger.LogInformation($"{nameof(UpdateAsync)}({request.UserName}) started...");

            if (_context.ApplicationUsers.AsQueryable()
                .All(u => u.NormalizedUserName != GetNormalized(request.UserName)))
                throw new DataException($"User with name {request.UserName} doesn't exist!");

            var user = await _context.ApplicationUsers.FirstOrDefaultAsync(
                u => u.NormalizedUserName == GetNormalized(request.UserName), token);


            var prevMail = user.NormalizedEmail;
            user.Email = request.Email;
            user.NormalizedEmail = GetNormalized(request.Email);
            user.PasswordHash = HashUtils.GetHash(request.Password, _config["Authorization:Salt"]);

            if (prevMail != GetNormalized(request.Email))
                await _confirmationService.SendConfirmationCode(user, token);

            _context.ApplicationUsers.Update(user);

            await _context.SaveChangesAsync(token);

            _logger.LogInformation($"{nameof(UpdateAsync)}({request.UserName}) finished...");
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(UpdateAsync)}({request.UserName}) error: {ex.Message}", ex);

            throw;
        }
    }

    public async Task DeleteAsync(UserDeleteRequest request, CancellationToken token)
    {
        try
        {
            _logger.LogInformation($"{nameof(DeleteAsync)}({request.UserName}) started...");

            if (_context.ApplicationUsers.AsQueryable()
                .All(u => u.NormalizedUserName != GetNormalized(request.UserName)))
                throw new DataException($"User with name {request.UserName} doesn't exist!");

            var user = await _context.ApplicationUsers.FirstOrDefaultAsync(
                u => u.NormalizedUserName == GetNormalized(request.UserName), token);

            _context.ApplicationUsers.Remove(user);

            await _context.SaveChangesAsync(token);

            _logger.LogInformation($"{nameof(DeleteAsync)}({request.UserName}) finished...");
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(DeleteAsync)}({request.UserName}) error: {ex.Message}", ex);

            throw;
        }
    }

    public async Task<UserGetResponse> GetAsync(UserGetRequest request, CancellationToken token)
    {
        try
        {
            _logger.LogInformation($"{nameof(GetAsync)}({request.UserName}) started...");

            if (_context.ApplicationUsers.AsQueryable()
                .All(u => u.NormalizedUserName != GetNormalized(request.UserName)))
                throw new DataException($"User with name {request.UserName} doesn't exist!");

            var user = await _context.ApplicationUsers.FirstOrDefaultAsync(
                u => u.NormalizedUserName == GetNormalized(request.UserName), token);

            _logger.LogInformation($"{nameof(GetAsync)}({request.UserName}) finished...");

            return new UserGetResponse
            {
                Email = user.Email,
                UserName = user.UserName
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(GetAsync)}({request.UserName}) error: {ex.Message}", ex);

            throw;
        }
    }

    public async Task<bool> ConfirmCodeAsync(string requestEmail, string requestToken, CancellationToken token)
    {
        if (_context.ApplicationUsers.AsQueryable()
            .All(u => u.NormalizedEmail != GetNormalized(requestEmail)))
            throw new DataException($"User with email {requestEmail} doesn't exist!");

        var user = await _context.ApplicationUsers.FirstOrDefaultAsync(
            u => u.NormalizedEmail == GetNormalized(requestEmail), token);

        return await _confirmationService.ConfirmCodeAsync(requestToken, user, token);
    }

    private static string GetNormalized(string input)
        => input?.ToUpper();
}