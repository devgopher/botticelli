using Botticelli.Auth.Data;
using Botticelli.Auth.Data.Models;
using Botticelli.Auth.Dto;
using Botticelli.Auth.Dto.Credentials;
using Botticelli.Auth.Dto.User;
using Botticelli.Auth.Rules;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Botticelli.Auth.Services;

public class BasicBotAuthService : BotAuthService<BotAuthCredentials, BotUser, BotUserInfo>
{
    public BasicBotAuthService(CredentialsRulesBuilder<BotAuthCredentials> credentialsRulesBuilder,
                                 AuthDefaultDbContext authDefaultDbContext) :
            base(credentialsRulesBuilder, authDefaultDbContext) =>
            credentialsRulesBuilder.AddRule(r => Users.AsNoTracking().Any(e => e.UserId == r.UserId && e.IsActive));

    protected override BotUserInfo GetDefaultUser(string defaultRoleName = DefaultRoles.Guest)
    {
       var defaultUser = Users.AsNoTracking().SingleOrDefault(e => e.UserId == defaultRoleName);
       
       return defaultUser.Adapt<BotUserInfo>();
    }

    protected override async Task<IdentifyResponse<BotUserInfo>> DoLogin(BotAuthCredentials dto)
    {
        var entity = await Users.AsNoTracking().FirstOrDefaultAsync(e => e.UserId == dto.UserId && e.IsActive);
        var adapted = entity.Adapt<BotUserInfo>();
        
        return entity == default ?
                new IdentifyResponse<BotUserInfo>(false, $"User with id: {dto.UserId} not found!", null) :
                new IdentifyResponse<BotUserInfo>(true, string.Empty, adapted);
    }
}