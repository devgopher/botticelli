using Botticelli.Auth.Dto.User;
using Botticelli.Auth.Mapping;
using Botticelli.Auth.Services;
using Botticelli.Auth.Tests.Helpers;
using FluentAssertions;
using NUnit.Framework;

namespace Botticelli.Auth.Tests.Services;

[TestFixture]
public class ManagerTests
{
    [SetUp]
    public void SetUp() => MappingProfile.UseProfile();

    [Test]
    public async Task DefaultUserManager_Should_AddAndGet()
    {
        await using var ctx = AuthDbFactory.Create();
        var roleId = Guid.Parse("9947e363-4255-408d-b277-33402b9f07a1");
        var manager = new DefaultUserManager(ctx);

        await manager.Add(new BotUserInfo
        {
            UserId = "u1",
            UserName = "User One",
            RoleId = roleId
        });

        (await manager.Get()).Should().Contain(u => u.UserId == "u1");
    }

    [Test]
    public async Task DefaultUserManager_Update_Should_PersistChanges()
    {
        var dbName = Guid.NewGuid().ToString();
        var roleId = Guid.Parse("9947e363-4255-408d-b277-33402b9f07a1");

        await using (var ctx = AuthDbFactory.Create(dbName))
        {
            await new DefaultUserManager(ctx).Add(new BotUserInfo
            {
                UserId = "u2",
                UserName = "User Two",
                RoleId = roleId
            });
        }

        await using (var ctx = AuthDbFactory.Create(dbName))
        {
            await new DefaultUserManager(ctx).Update(new BotUserInfo
            {
                UserId = "u2",
                UserName = "Updated",
                RoleId = roleId
            });
        }

        await using (var ctx = AuthDbFactory.Create(dbName))
        {
            (await new DefaultUserManager(ctx).Get()).Should().Contain(u => u.UserName == "Updated");
        }
    }

    [Test]
    public async Task DefaultUserRoleManager_Should_AddAndGet()
    {
        await using var ctx = AuthDbFactory.Create();
        var manager = new DefaultUserRoleManager(ctx);
        var id = Guid.NewGuid();

        await manager.Add(new BotUserRoleInfo
        {
            Id = id,
            RoleName = "Custom",
            Description = "desc",
            IsSuperUser = false
        });

        (await manager.Get()).Should().Contain(r => r.Id == id && r.RoleName == "Custom");
    }
}
