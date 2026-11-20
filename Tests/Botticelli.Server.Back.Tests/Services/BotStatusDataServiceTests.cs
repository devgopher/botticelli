using Botticelli.Server.Back.Services;
using Botticelli.Server.Back.Tests.Helpers;
using Botticelli.Server.Data.Entities.Bot;
using Botticelli.Shared.API.Admin.Responses;
using Botticelli.Shared.Constants;
using FluentAssertions;
using NUnit.Framework;

namespace Botticelli.Server.Back.Tests.Services;

[TestFixture]
public class BotStatusDataServiceTests
{
    [Test]
    public async Task GetBots_Should_ReturnAll()
    {
        await using var context = TestServerDataContextFactory.Create();
        context.BotInfos.AddRange(
            new BotInfo { BotId = "1", BotName = "A", Type = BotType.Telegram, Status = BotStatus.Unlocked },
            new BotInfo { BotId = "2", BotName = "B", Type = BotType.Telegram, Status = BotStatus.Locked });
        await context.SaveChangesAsync();
        var service = new BotStatusDataService(context);

        service.GetBots().Should().HaveCount(2);
    }

    [Test]
    public async Task GetRequiredBotStatus_Should_ReturnStatusOrUnknown()
    {
        await using var context = TestServerDataContextFactory.Create();
        context.BotInfos.Add(new BotInfo
        {
            BotId = "bot-1",
            BotName = "A",
            Type = BotType.Telegram,
            Status = BotStatus.Unlocked
        });
        await context.SaveChangesAsync();
        var service = new BotStatusDataService(context);

        (await service.GetRequiredBotStatus("bot-1")).Should().Be(BotStatus.Unlocked);
        (await service.GetRequiredBotStatus("missing")).Should().Be(BotStatus.Unknown);
    }

    [Test]
    public async Task GetRequiredBotKey_Should_ReturnKeyOrEmpty()
    {
        await using var context = TestServerDataContextFactory.Create();
        context.BotInfos.Add(new BotInfo
        {
            BotId = "bot-1",
            BotName = "A",
            Type = BotType.Telegram,
            Status = BotStatus.Unlocked,
            BotKey = "secret"
        });
        await context.SaveChangesAsync();
        var service = new BotStatusDataService(context);

        (await service.GetRequiredBotKey("bot-1")).Should().Be("secret");
        (await service.GetRequiredBotKey("missing")).Should().BeEmpty();
    }

    [Test]
    public async Task GetBotInfo_Should_ReturnEntityOrNull()
    {
        await using var context = TestServerDataContextFactory.Create();
        context.BotInfos.Add(new BotInfo
        {
            BotId = "bot-1",
            BotName = "A",
            Type = BotType.Telegram,
            Status = BotStatus.Unlocked
        });
        await context.SaveChangesAsync();
        var service = new BotStatusDataService(context);

        (await service.GetBotInfo("bot-1"))!.BotName.Should().Be("A");
        (await service.GetBotInfo("missing")).Should().BeNull();
    }
}
