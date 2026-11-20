using Botticelli.Server.Data.Entities.Bot;
using Botticelli.Server.Data.Entities.Bot.Broadcasting;
using Botticelli.Server.Data.Tests.Helpers;
using Botticelli.Shared.API.Admin.Responses;
using Botticelli.Shared.Constants;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Botticelli.Server.Data.Tests;

[TestFixture]
public class ServerDataContextTests
{
    [Test]
    public void ParameterlessConstructor_Should_CreateSqliteContext()
    {
        using var context = new ServerDataContext();
        context.Should().NotBeNull();
        context.BotInfos.Should().NotBeNull();
    }

    [Test]
    public async Task Context_Should_PersistBotInfo()
    {
        await using var context = TestServerDataContextFactory.Create();
        context.BotInfos.Add(new BotInfo
        {
            BotId = "bot-1",
            BotName = "Test",
            Status = BotStatus.Unknown,
            Type = BotType.Telegram,
            BotKey = "key"
        });
        await context.SaveChangesAsync();

        var loaded = await context.BotInfos.SingleAsync();
        loaded.BotId.Should().Be("bot-1");
        loaded.BotName.Should().Be("Test");
        loaded.Type.Should().Be(BotType.Telegram);
    }

    [Test]
    public async Task Context_Should_PersistBroadcastWithAttachment()
    {
        await using var context = TestServerDataContextFactory.Create();
        var broadcastId = Guid.NewGuid().ToString();
        context.BroadcastMessages.Add(new Broadcast
        {
            Id = broadcastId,
            BotId = "bot-1",
            Body = "hello",
            Attachments =
            [
                new BroadcastAttachment
                {
                    Id = Guid.NewGuid(),
                    BroadcastId = broadcastId,
                    MediaType = MediaType.Voice,
                    Filename = "a.ogg",
                    Content = [9]
                }
            ]
        });
        await context.SaveChangesAsync();

        var loaded = await context.BroadcastMessages.Include(b => b.Attachments).SingleAsync();
        loaded.Body.Should().Be("hello");
        loaded.Attachments.Should().ContainSingle(a => a.Filename == "a.ogg");
    }

    [Test]
    public async Task Context_Should_SeedApplicationRoles()
    {
        await using var context = TestServerDataContextFactory.Create();

        var roles = await context.ApplicationRoles.ToListAsync();
        roles.Should().HaveCount(3);
        roles.Select(r => r.Name).Should().BeEquivalentTo("admin", "bot_manager", "viewer");
    }

    [Test]
    public async Task Context_Should_PersistIdentityUserAndRoleLink()
    {
        await using var context = TestServerDataContextFactory.Create();
        var userId = Guid.NewGuid().ToString();
        var roleId = (await context.ApplicationRoles.FirstAsync()).Id;

        context.ApplicationUsers.Add(new IdentityUser<string>
        {
            Id = userId,
            UserName = "admin",
            NormalizedUserName = "ADMIN",
            Email = "a@b.c",
            NormalizedEmail = "A@B.C"
        });
        context.ApplicationUserRoles.Add(new IdentityUserRole<string>
        {
            UserId = userId,
            RoleId = roleId
        });
        await context.SaveChangesAsync();

        (await context.ApplicationUsers.CountAsync()).Should().Be(1);
        (await context.ApplicationUserRoles.CountAsync()).Should().Be(1);
    }
}
