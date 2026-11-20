using Botticelli.Server.Back.Services;
using Botticelli.Server.Back.Tests.Helpers;
using Botticelli.Server.Data.Entities.Bot;
using Botticelli.Shared.API.Admin.Responses;
using Botticelli.Shared.Constants;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace Botticelli.Server.Back.Tests.Services;

[TestFixture]
public class BotManagementServiceTests
{
    [Test]
    public async Task RegisterBot_Should_AddNewBot()
    {
        await using var context = TestServerDataContextFactory.Create();
        var service = new BotManagementService(context, NullLogger<BotManagementService>.Instance);

        var ok = await service.RegisterBot("bot-1", "key", "Name", BotType.Telegram);

        ok.Should().BeTrue();
        context.BotInfos.Should().ContainSingle(b => b.BotId == "bot-1" && b.BotName == "Name");
    }

    [Test]
    public async Task RegisterBot_Should_Succeed_WhenAlreadyExists()
    {
        await using var context = TestServerDataContextFactory.Create();
        context.BotInfos.Add(new BotInfo
        {
            BotId = "bot-1",
            BotName = "Existing",
            Type = BotType.Telegram,
            Status = BotStatus.Unlocked
        });
        await context.SaveChangesAsync();
        var service = new BotManagementService(context, NullLogger<BotManagementService>.Instance);

        var ok = await service.RegisterBot("bot-1", "key", "Name", BotType.Telegram);

        ok.Should().BeTrue();
        context.BotInfos.Should().ContainSingle();
    }

    [Test]
    public async Task SetRequiredBotStatus_Should_UpdateStatus()
    {
        await using var context = TestServerDataContextFactory.Create();
        context.BotInfos.Add(new BotInfo
        {
            BotId = "bot-1",
            BotName = "N",
            Type = BotType.Telegram,
            Status = BotStatus.Unknown
        });
        await context.SaveChangesAsync();
        var service = new BotManagementService(context, NullLogger<BotManagementService>.Instance);

        await service.SetRequiredBotStatus("bot-1", BotStatus.Unlocked);

        context.BotInfos.Single().Status.Should().Be(BotStatus.Unlocked);
    }

    [Test]
    public async Task SetKeepAlive_Should_UpdateTimestamp()
    {
        await using var context = TestServerDataContextFactory.Create();
        context.BotInfos.Add(new BotInfo
        {
            BotId = "bot-1",
            BotName = "N",
            Type = BotType.Telegram,
            Status = BotStatus.Unlocked
        });
        await context.SaveChangesAsync();
        var service = new BotManagementService(context, NullLogger<BotManagementService>.Instance);

        await service.SetKeepAlive("bot-1");

        context.BotInfos.Single().LastKeepAlive.Should().NotBeNull();
    }

    [Test]
    public async Task RemoveBot_Should_DeleteBot()
    {
        await using var context = TestServerDataContextFactory.Create();
        context.BotInfos.Add(new BotInfo
        {
            BotId = "bot-1",
            BotName = "N",
            Type = BotType.Telegram,
            Status = BotStatus.Locked
        });
        await context.SaveChangesAsync();
        var service = new BotManagementService(context, NullLogger<BotManagementService>.Instance);

        await service.RemoveBot("bot-1");

        context.BotInfos.Should().BeEmpty();
    }

    [Test]
    public async Task UpdateBot_Should_UpdateKeyAndName()
    {
        await using var context = TestServerDataContextFactory.Create();
        context.BotInfos.Add(new BotInfo
        {
            BotId = "bot-1",
            BotName = "Old",
            BotKey = "old-key",
            Type = BotType.Telegram,
            Status = BotStatus.Locked
        });
        await context.SaveChangesAsync();
        var service = new BotManagementService(context, NullLogger<BotManagementService>.Instance);

        var ok = await service.UpdateBot("bot-1", "new-key", "New");

        ok.Should().BeTrue();
        var bot = context.BotInfos.Single();
        bot.BotKey.Should().Be("new-key");
        bot.BotName.Should().Be("New");
        bot.Status.Should().Be(BotStatus.Locked);
    }

    [Test]
    public async Task UpdateBot_Should_ReturnFalse_WhenMissing()
    {
        await using var context = TestServerDataContextFactory.Create();
        var service = new BotManagementService(context, NullLogger<BotManagementService>.Instance);

        var ok = await service.UpdateBot("missing", "k", "n");

        ok.Should().BeFalse();
    }
}
