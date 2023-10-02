using Botticelli.Server.Data;
using Botticelli.Server.Data.Entities.Auth;
using Botticelli.Server.Data.Exceptions;
using Botticelli.Server.Settings;
using Botticelli.Shared.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Botticelli.Server.Services.Auth;

public class UserService : IUserService
{
    private readonly IConfiguration _config;
    private readonly BotInfoContext _context;
    private readonly ILogger<AdminAuthService> _logger;

    public UserService(IConfiguration config,
        IHttpContextAccessor httpContextAccessor,
        BotInfoContext context,
        ILogger<AdminAuthService> logger,
        IOptionsMonitor<ServerSettings> settings)
    {
        _config = config;
        _context = context;
        _logger = logger;
    }

    public async Task AddAsync(UserAddRequest request, CancellationToken token)
    {
        try
        {
            _logger.LogInformation($"{nameof(AddAsync)}({request.UserName}) started...");

            if (_context.ApplicationUsers.AsQueryable()
                .Any(u => u.NormalizedEmail == GetNormalized(request.Email)))
                throw new DataException($"User with email {request.Email} already exists!");

            var user = new IdentityUser
            {
                Id = Guid.NewGuid().ToString(),
                Email = request.Email,
                NormalizedEmail = GetNormalized(request.Email),
                UserName = request.Email,
                NormalizedUserName = GetNormalized(request.Email),
                PasswordHash = HashUtils.GetHash(request.Password, _config["Authorization:Salt"])
            };

            // Temporary - because now we assume, that we've only a single role - "admin"! 
            var appRole = new IdentityUserRole<string>
            {
                UserId = user.Id,
                RoleId = _context.ApplicationRoles.FirstOrDefault()?.Id ?? "-1"
            };

            await _context.ApplicationUsers.AddAsync(user, token);
            await _context.ApplicationUserRoles.AddAsync(appRole, token);

            await _context.SaveChangesAsync(token);

            _logger.LogInformation($"{nameof(AddAsync)}({request.UserName}) finished...");
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(AddAsync)}({request.UserName}) error: {ex.Message}", ex);
        }
    }

    public async Task UpdateAsync(UserUpdateRequest request, CancellationToken token)
    {
        try
        {
            _logger.LogInformation($"{nameof(UpdateAsync)}({request.UserName}) started...");

            if (_context.ApplicationUsers.AsQueryable()
                .All(u => u.NormalizedEmail != GetNormalized(request.Email)))
                throw new DataException($"User with email {request.Email} doesn't exist!");

            var user = await _context.ApplicationUsers.FirstOrDefaultAsync(
                u => u.NormalizedEmail == GetNormalized(request.Email), token);

            user.UserName = request.UserName;
            user.PasswordHash = HashUtils.GetHash(request.Password, _config["Authorization:Salt"]);

            _context.ApplicationUsers.Update(user);

            await _context.SaveChangesAsync(token);

            _logger.LogInformation($"{nameof(UpdateAsync)}({request.UserName}) finished...");
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(UpdateAsync)}({request.UserName}) error: {ex.Message}", ex);
        }
    }

    public async Task DeleteAsync(UserDeleteRequest request, CancellationToken token)
    {
        try
        {
            _logger.LogInformation($"{nameof(DeleteAsync)}({request.Email}) started...");

            if (_context.ApplicationUsers.AsQueryable()
                .All(u => u.NormalizedEmail != GetNormalized(request.Email)))
                throw new DataException($"User with email {request.Email} doesn't exist!");

            var user = await _context.ApplicationUsers.FirstOrDefaultAsync(
                u => u.NormalizedEmail == GetNormalized(request.Email), token);

            _context.ApplicationUsers.Remove(user);

            await _context.SaveChangesAsync(token);

            _logger.LogInformation($"{nameof(DeleteAsync)}({request.Email}) finished...");
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(DeleteAsync)}({request.Email}) error: {ex.Message}", ex);
        }
    }

    public async Task<UserGetResponse> GetAsync(UserGetRequest request, CancellationToken token)
    {
        try
        {
            _logger.LogInformation($"{nameof(GetAsync)}({request.Email}) started...");

            if (_context.ApplicationUsers.AsQueryable()
                .All(u => u.NormalizedEmail != GetNormalized(request.Email)))
                throw new DataException($"User with email {request.Email} doesn't exist!");

            var user = await _context.ApplicationUsers.FirstOrDefaultAsync(
                u => u.NormalizedEmail == GetNormalized(request.Email), token);

            _logger.LogInformation($"{nameof(GetAsync)}({request.Email}) finished...");

            return new UserGetResponse
            {
                Email = user.Email,
                UserName = user.UserName
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(GetAsync)}({request.Email}) error: {ex.Message}", ex);
        }

        return default;
    }

    private static string GetNormalized(string input)
        => input?.ToUpper();
}