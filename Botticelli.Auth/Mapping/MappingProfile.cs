using Botticelli.Auth.Data.Models;
using Botticelli.Auth.Dto.User;
using Mapster;

namespace Botticelli.Auth.Mapping;

public static class MappingProfile
{
    public static void UseProfile()
    {
        TypeAdapterConfig<BotUser, BotUserInfo>.NewConfig()
                                               .IgnoreNullValues(true);

        TypeAdapterConfig<BotUserInfo, BotUser>.NewConfig()
                                               .Map(dest => dest.IsActive, src => true)
                                               .IgnoreNullValues(true);
    }
}