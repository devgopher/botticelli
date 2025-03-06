using Botticelli.Auth.Data;
using Botticelli.Auth.Data.Models;
using Botticelli.Auth.Dto;
using Botticelli.Auth.Dto.Credentials;
using Botticelli.Auth.Rules;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Botticelli.Auth.Services;

public abstract class BotAuthService<TDto, TEntity, TUserInfo>(
        CredentialsRulesBuilder<TDto> credentialsRulesBuilder,
        AuthDefaultDbContext authDefaultDbContext)
        : IIdentifier<TDto, TUserInfo>
        where TDto : IBotAuthCredentials
        where TEntity : class
{
    protected readonly DbSet<TEntity> Users = authDefaultDbContext.Set<TEntity>();
    private readonly DbSet<AccessHistory<TEntity>> _accessHistory = authDefaultDbContext.Set<AccessHistory<TEntity>>();
    private readonly CredentialsRules<TDto> _credentialsRules = credentialsRulesBuilder.Build();

    public async Task<IdentifyResponse<TUserInfo>> Identify(TDto dto)
    {
        IdentifyResponse<TUserInfo> response;

        if (await _credentialsRules.Compare(dto))
        {
            response = await DoLogin(dto);
            
            await _accessHistory.AddAsync(new AccessHistory<TEntity>
            {
                TimestampUtc = DateTime.UtcNow,
                IsSuccess = true,
                ErrorMessage = string.Empty,
                Entity = dto.Adapt<TEntity>()
            });
        }
        else
        {
            response = new IdentifyResponse<TUserInfo>(false, "Invalid credentials or inactive user!", GetDefaultUser());

            await _accessHistory.AddAsync(new AccessHistory<TEntity>
            {
                TimestampUtc = DateTime.UtcNow,
                IsSuccess = false,
                ErrorMessage = "Invalid credentials or inactive user!"
            });
        }

        await authDefaultDbContext.SaveChangesAsync();

        return response;
    }

    protected abstract TUserInfo? GetDefaultUser(string defaultRoleName = DefaultRoles.Guest);
    
    protected abstract Task<IdentifyResponse<TUserInfo>> DoLogin(TDto dto);
}