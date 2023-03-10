using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Botticelli.Server.Data;
using Botticelli.Server.Data.Entities.Auth;
using Botticelli.Server.Data.Exceptions;
using Botticelli.Shared.Utils;

namespace Botticelli.Server.Services.Auth
{
    public class AuthService
    {
        private readonly DbSet<IdentityUser<string>> _userRepository;
        private readonly DbSet<IdentityUserRole<string>> _appUserRoleRepository;
        private readonly DbSet<IdentityRole<string>> _roleRepository;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly BotInfoContext _context;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IConfiguration config,
                           IHttpContextAccessor httpContextAccessor,
                           BotInfoContext context,
                           DbSet<IdentityUserRole<string>> appUserRoleRepository,
                           DbSet<IdentityUser<string>> userRepository,
                           DbSet<IdentityRole<string>> roleRepository,
                           ILogger<AuthService> logger)
        {
            _config = config;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _appUserRoleRepository = appUserRoleRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _logger = logger;
        }


        public async Task Register(UserRegisterPost userRegister)
        {
            try
            {
                _logger.LogInformation($"{nameof(Register)}({userRegister.UserName}) started...");

                if (_userRepository.AsQueryable()
                                   .Any(u => u.NormalizedEmail == GetNormalized(userRegister.Email)))
                    throw new DataException($"User with email {userRegister.Email} already exists!");

                var user = new IdentityUser()
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = userRegister.Email,
                    NormalizedEmail = GetNormalized(userRegister.Email),
                    UserName = userRegister.Email,
                    NormalizedUserName = GetNormalized(userRegister.Email),
                    PasswordHash = HashUtils.GetHash(userRegister.Password, _config["Authorization:Salt"])
                };

                // Temporary - because now we assume, that we've only a single role - "farm director"! 
                var appRole = new IdentityUserRole<string>()
                {
                    UserId = user.Id,
                    RoleId = _roleRepository.FirstOrDefault().Id
                };

                await _appUserRoleRepository.AddAsync(appRole);

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
            if (!CheckAccess(login))
                return "Wrong login/password!";

            var user = _userRepository.AsQueryable().FirstOrDefault(u => u.NormalizedEmail == GetNormalized(login.Email),
                u => u.UserRoles);

            var roleName = string.Empty;

            var userRole = user.UserRoles.FirstOrDefault();
            if (userRole != null)
            {
                var role = _appUserRoleRepository.FirstOrDefault(userRole.RoleId);
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
            var pureToken = token.Split(" ")[1];

            var sign = _config["Authorization:Key"];
            try
            {
                var handler = new JwtSecurityTokenHandler();
                SecurityToken tk;
                handler.ValidateToken(token, new TokenValidationParameters()
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(sign)),
                    ValidIssuer = _config["Authorization:Issuer"],
                    ValidateAudience = false
                }, out tk);
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
            return _userRepository.Any(u =>
                u.NormalizedEmail == normalizedEmail && u.PasswordHash == hashedPassword && u.StatusId == 1);
        }

        public ApplicationUserGet Get()
        {
            var userId = GetCurrentUserId();
            var user = _userRepository.FirstOrDefault(u => u.Id == userId);
            var user2roles = _appUserRoleRepository.Get(ur => ur.UserId == userId).ToArray();
            user.UserRoleIds = user2roles.Select(ur => ur.RoleId).ToHashSet();

            return Mapper.Map<ApplicationUserGet>(user);
        }

        public async Task Update(ApplicationUserPut model)
        {
            var user = _userRepository.FirstOrDefault(u => u.Id == GetCurrentUserId());
            user.FarmId = model.FarmId;
            user.FullName = model.FullName;
            user.UserRoles = _appUserRoleRepository.Get(ur => model.UserRoleIds.Contains(ur.RoleId)).ToList();

            _userRepository.Update(user);
            await _context.SaveChangesAsync();
        }

        public string GetCurrentUserName()
            => _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "applicationUserName")?.Value;

        public Guid GetCurrentUserId()
            => Guid.Parse(_httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "applicationUserId")?.Value);

        public Guid GetCurrentUserFarmId()
            => Guid.Parse(_httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "farmId")?.Value);

        public string GetCurrentUserRoleName()
            => _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "role")?.Value;

        public bool IsAdmin()
            => _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value.ToUpper() == "ADMIN";

        private static string GetNormalized(string input)
            => input.ToUpper();
    }
}
