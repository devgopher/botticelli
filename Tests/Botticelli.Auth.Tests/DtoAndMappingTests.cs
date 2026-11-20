using Botticelli.Auth.Data.Models;
using Botticelli.Auth.Dto;
using Botticelli.Auth.Dto.Credentials;
using Botticelli.Auth.Dto.User;
using Botticelli.Auth.Mapping;
using FluentAssertions;
using Mapster;
using NUnit.Framework;

namespace Botticelli.Auth.Tests;

[TestFixture]
public class DtoAndMappingTests
{
    [Test]
    public void CredentialsAndResponses_Should_HoldValues()
    {
        var creds = new BotAuthCredentials { UserId = "id-1" };
        creds.UserId.Should().Be("id-1");

        var response = new IdentifyResponse<BotUserInfo>(true, "", new BotUserInfo
        {
            UserId = "id-1",
            UserName = "n",
            RoleId = Guid.NewGuid()
        });
        response.Success.Should().BeTrue();
        response.User!.UserId.Should().Be("id-1");
    }

    [Test]
    public void MappingProfile_Should_MapBotUserBothWays()
    {
        MappingProfile.UseProfile();
        var roleId = Guid.NewGuid();
        var entity = new BotUser
        {
            UserId = "u",
            UserName = "name",
            RoleId = roleId,
            IsActive = false,
            Email = "a@b.c"
        };

        var info = entity.Adapt<BotUserInfo>();
        info.UserId.Should().Be("u");
        info.Email.Should().Be("a@b.c");

        var back = info.Adapt<BotUser>();
        back.IsActive.Should().BeTrue();
        back.UserName.Should().Be("name");
    }

    [Test]
    public void BotUserRoleInfo_Should_ExposeFlags()
    {
        var role = new BotUserRoleInfo
        {
            Id = Guid.NewGuid(),
            RoleName = "Admin",
            Description = "d",
            IsSuperUser = true
        };
        role.IsSuperUser.Should().BeTrue();
    }
}
