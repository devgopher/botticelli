using Botticelli.Server.Data;
using Botticelli.Server.Data.Entities.Auth;
using Botticelli.Server.Data.Exceptions;
using Botticelli.Shared.Utils;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Botticelli.Server.Services.Auth;

public class AuthService
{
    private readonly IConfiguration _config;
    private readonly BotInfoContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<AuthService> _logger;
    private readonly IMapper _mapper;

    public AuthService(IConfiguration config,
                       IHttpContextAccessor httpContextAccessor,
                       BotInfoContext context,
                       ILogger<AuthService> logger,
                       IMapper mapper)
    {
        _config = config;
        _httpContextAccessor = httpContextAccessor;
        _context = context;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task Register(UserRegisterPost userRegister)
    {
        try
        {
            _logger.LogInformation($"{nameof(Register)}({userRegister.UserName}) started...");

            if (_context.ApplicationUsers.AsQueryable()
                        .Any(u => u.NormalizedEmail == GetNormalized(userRegister.Email)))
                throw new DataException($"User with email {userRegister.Email} already exists!");

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

            await _context.ApplicationUserRoles.AddAsync(appRole);

            await _context.SaveChangesAsync();

            _logger.LogInformation($"{nameof(Register)}({userRegister.UserName}) finished...");
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(Register)}({userRegister.UserName}) error: {ex.Message}", ex);
        }
    }

    public string GenerateToken(UserLoginPost login, int timeInMinutes = 10 * 12 * 60)
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
            new Claim("role", roleName)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Authorization:Key"]));
        var signCreds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(_config["Authorization:Issuer"],
                                         _config["Authorization:Issuer"],
                                         claims,
                                         expires: DateTime.Now.AddMinutes(timeInMinutes),
                                         signingCredentials: signCreds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

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

    public bool CheckAccess(UserLoginPost login)
    {
        var hashedPassword = HashUtils.GetHash(login.Password, _config["Authorization:Salt"]);
        var normalizedEmail = GetNormalized(login.Email);

        return _context.ApplicationUsers.Any(u =>
                                                     u.NormalizedEmail == normalizedEmail && u.PasswordHash == hashedPassword);
    }

    public ApplicationUserGet Get()
    {
        var userId = GetCurrentUserId();
        var user = _context.ApplicationUsers
                           .AsQueryable()
                           .FirstOrDefault(u => u.Id == userId);

        return _mapper.Map<ApplicationUserGet>(user);
    }

    public async Task Update(ApplicationUserPut model)
    {
        var user = _context.ApplicationUsers.FirstOrDefault(u => u.Id == GetCurrentUserId());
        user.UserName = model.UserName;

        _context.ApplicationUsers.Update(user);
        await _context.SaveChangesAsync();
    }

    public string GetCurrentUserName()
        => _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "applicationUserName")?.Value;

    public string? GetCurrentUserId()
        => _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "applicationUserId")?.Value;

    public string GetCurrentUserRoleName()
        => _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "role")?.Value;

    public bool IsAdmin()
        => _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value.ToUpper() == "ADMIN";

    private static string GetNormalized(string input)
        => input.ToUpper();
}