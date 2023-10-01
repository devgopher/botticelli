using Botticelli.Server.Data;
using Botticelli.Server.Data.Entities.Auth;
using Botticelli.Server.Data.Exceptions;
using Botticelli.Server.Settings;
using Botticelli.Shared.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Botticelli.Server.Services.Auth;

public class UserService : IUserService
{
    private readonly IConfiguration _config;
    private readonly BotInfoContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<AdminAuthService> _logger;
    private readonly IOptionsMonitor<ServerSettings> _settings;

    public UserService(IConfiguration config,
        IHttpContextAccessor httpContextAccessor,
        BotInfoContext context,
        ILogger<AdminAuthService> logger,
        IOptionsMonitor<ServerSettings> settings)
    {
        _config = config;
        _httpContextAccessor = httpContextAccessor;
        _context = context;
        _logger = logger;
        _settings = settings;
    }

    public async Task AddAsync(UserAddRequest request)
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

            await _context.ApplicationUsers.AddAsync(user);
            await _context.ApplicationUserRoles.AddAsync(appRole);

            await _context.SaveChangesAsync();

            _logger.LogInformation($"{nameof(AddAsync)}({request.UserName}) finished...");
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(AddAsync)}({request.UserName}) error: {ex.Message}", ex);
        }
    }

    public Task UpdateAsync(UserUpdateRequest request) => throw new NotImplementedException();

    public Task DeleteAsync(UserDeleteRequest request) => throw new NotImplementedException();

    public Task<UserGetResponse> GetAsync(UserGetRequest request) => throw new NotImplementedException();

    private static string GetNormalized(string input)
        => input?.ToUpper();
}