using Botticelli.Auth.Data;
using Botticelli.Auth.Data.Models;
using Botticelli.Auth.Dto.User;

namespace Botticelli.Auth.Services;

public class DefaultUserManager(AuthDefaultDbContext authDefaultDbContext)
        : Manager<BotUserInfo, BotUser>(authDefaultDbContext)
{
    
}