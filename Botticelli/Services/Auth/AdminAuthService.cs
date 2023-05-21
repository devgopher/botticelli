using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using Botticelli.Server.Data;
using Botticelli.Server.Data.Entities.Auth;
using Botticelli.Server.Data.Exceptions;
using Botticelli.Server.Settings;
using Botticelli.Shared.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Botticelli.Server.Services.Auth;

/// <summary>
/// Authentication service
/// </summary>
public class AuthService
{
    private readonly IConfiguration _config;
    private readonly BotInfoContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<AuthService> _logger;
    private readonly IOptionsMonitor<ServerSettings> _settings;

    public AuthService(IConfiguration config,
                       IHttpContextAccessor httpContextAccessor,
                       BotInfoContext context,
                       ILogger<AuthService> logger,
                       IOptionsMonitor<ServerSettings> settings)
    {
        _config = config;
        _httpContextAccessor = httpContextAccessor;
        _context = context;
        _logger = logger;
        _settings = settings;
    }

    /// <summary>
    /// Register an admin user
    /// </summary>
    /// <param name="userRegister"></param>
    /// <returns></returns>
    /// <exception cref="DataException"></exception>
    public async Task RegisterAsync(UserRegisterPost userRegister)
    {
        try
        {
            _logger.LogInformation($"{nameof(RegisterAsync)}({userRegister.UserName}) started...");

            if (_context.ApplicationUsers.AsQueryable()
                        .Any(u => u.NormalizedEmail == GetNormalized(userRegister.Email)))
                throw new DataException($"User with Email {userRegister.Email} already exists!");

            var user = new IdentityUser
            {
                Id = Guid.NewGuid().ToString(),
                Email = userRegister.Email,
                NormalizedEmail = GetNormalized(userRegister.Email),
                UserName = userRegister.Email,
                NormalizedUserName = GetNormalized(userRegister.Email),
                PasswordHash = HashUtils.GetHash(userRegister.Password, _config["Authorization:Salt"])
            };

            // Temporary - because now we assume, that we've only a single role - "farm director"! 
            var appRole = new IdentityUserRole<string>
            {
                UserId = user.Id,
                RoleId = _context.ApplicationRoles.FirstOrDefault()?.Id ?? "-1"
            };

            await _context.ApplicationUsers.AddAsync(user);
            await _context.ApplicationUserRoles.AddAsync(appRole);

            await _context.SaveChangesAsync();

            _logger.LogInformation($"{nameof(RegisterAsync)}({userRegister.UserName}) finished...");
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(RegisterAsync)}({userRegister.UserName}) error: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Generates auth token
    /// </summary>
    /// <param name="login"></param>
    /// <returns></returns>
    public string GenerateToken(UserLoginPost login)
    {
        if (!CheckAccess(login)) return "Wrong login/password!";

        var user = _context.ApplicationUsers
                           .AsQueryable()
                           .FirstOrDefault(u => u.NormalizedEmail == GetNormalized(login.Email));

        var userRole = _context.ApplicationUserRoles
                               .AsQueryable()
                               .FirstOrDefault(ur => ur.UserId == user.Id);

        var roleName = string.Empty;

        if (userRole != null)
        {
            var role = _context.ApplicationRoles
                               .AsQueryable()
                               .FirstOrDefault(r => r.Id == userRole.RoleId);

            roleName = role?.Name;
        }

        var claims = new[]
        {
            new Claim("applicationUserId", user.Id),
            new Claim("applicationUserName", user.UserName),
            new Claim("role", roleName ?? "no_role")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Authorization:Key"]));
        var signCreds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(_config["Authorization:Issuer"],
                                         _config["Authorization:Audience"],
                                         claims,
                                         expires: DateTime.Now.AddMinutes(_settings.CurrentValue.TokenLifetimeMin),
                                         signingCredentials: signCreds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }


    /// <summary>
    /// Checks auth token
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public bool CheckToken(string token)
    {
        var sign = _config["Authorization:Key"];

        try
        {
            var handler = new JwtSecurityTokenHandler();
            handler.ValidateToken(token,
                                  new TokenValidationParameters
                                  {
                                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(sign)),
                                      ValidIssuer = _config["Authorization:Issuer"],
                                      ValidateAudience = false
                                  },
                                  out _);
        }
        catch (Exception e)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Checks access
    /// </summary>
    /// <param name="login"></param>
    /// <returns></returns>
    public bool CheckAccess(UserLoginPost login)
    {
        var hashedPassword = HashUtils.GetHash(login.Password, _config["Authorization:Salt"]);
        var normalizedEmail = GetNormalized(login.Email);

        return _context.ApplicationUsers.Any(u =>
                                                     u.NormalizedEmail == normalizedEmail && u.PasswordHash == hashedPassword);
    }

    public string? GetCurrentUserId()
        => _httpContextAccessor.HttpContext
                               .User
                               .Claims
                               .FirstOrDefault(c => c.Type == "applicationUserId")?.Value;

    private static string GetNormalized(string input)
        => input?.ToUpper();
}