using Botticelli.Pay.Models;
using Botticelli.Pay.Processors;
using Botticelli.Pay.Telegram.Handlers;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Telegram.Bot;
using Telegram.Bot.Types;
using TgPreCheckoutQuery = Telegram.Bot.Types.Payments.PreCheckoutQuery;
using TgUser = Telegram.Bot.Types.User;

namespace Botticelli.Pay.Telegram.Tests.Handlers;

[TestFixture]
public class BotPreCheckoutSubHandlerTests
{
    private Mock<ILogger<BotPreCheckoutSubHandler>> _logger = null!;
    private Mock<ITelegramBotClient> _client = null!;

    [SetUp]
    public void SetUp()
    {
        _logger = new Mock<ILogger<BotPreCheckoutSubHandler>>();
        _client = new Mock<ITelegramBotClient>();
    }

    [Test]
    public async Task Process_WhenPreCheckoutQueryNull_Should_Return()
    {
        var handler = new BotPreCheckoutSubHandler(_logger.Object);

        await handler.Process(_client.Object, new Update(), CancellationToken.None);

        _client.VerifyNoOtherCalls();
    }

    [Test]
    public async Task Process_WithQuery_Should_RunChainAndAnswer()
    {
        var runner = new PayChainRunner<BotPreCheckoutSubHandler, PreCheckoutQuery>(
            [new DummyPayProcessor<BotPreCheckoutSubHandler, PreCheckoutQuery>()]);
        var handler = new BotPreCheckoutSubHandler(_logger.Object, runner);

        var update = new Update
        {
            PreCheckoutQuery = new TgPreCheckoutQuery
            {
                Id = "q-1",
                From = new TgUser
                {
                    Id = 42,
                    FirstName = "Ann",
                    LastName = "Lee",
                    Username = "ann",
                    IsBot = false
                },
                Currency = "USD",
                TotalAmount = 1500,
                InvoicePayload = "payload-1"
            }
        };

        var act = async () => await handler.Process(_client.Object, update, CancellationToken.None);

        await act.Should().NotThrowAsync();
    }

    [Test]
    public async Task Process_WhenRunnerFails_Should_StillComplete()
    {
        var failing = new Mock<IPayProcessor<BotPreCheckoutSubHandler, PreCheckoutQuery>>();
        failing.Setup(p => p.Process(It.IsAny<PreCheckoutQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync((false, "denied"));

        var runner = new PayChainRunner<BotPreCheckoutSubHandler, PreCheckoutQuery>([failing.Object]);
        var handler = new BotPreCheckoutSubHandler(_logger.Object, runner);

        var update = new Update
        {
            PreCheckoutQuery = new TgPreCheckoutQuery
            {
                Id = "q-2",
                From = new TgUser { Id = 1, FirstName = "Bob", IsBot = false },
                Currency = "EUR",
                TotalAmount = 100,
                InvoicePayload = "payload-2"
            }
        };

        await handler.Process(_client.Object, update, CancellationToken.None);
    }

    [Test]
    public async Task Process_WhenException_Should_LogAndSwallow()
    {
        var handler = new BotPreCheckoutSubHandler(_logger.Object);

        var update = new Update
        {
            PreCheckoutQuery = new TgPreCheckoutQuery
            {
                Id = "q-3",
                From = new TgUser { Id = 1, FirstName = "X", IsBot = false },
                Currency = "USD",
                TotalAmount = 1,
                InvoicePayload = ""
            }
        };

        var act = async () => await handler.Process(_client.Object, update, CancellationToken.None);

        await act.Should().NotThrowAsync();
    }

    [Test]
    public void Constructors_Should_CreateHandler()
    {
        var withDefaultRunner = new BotPreCheckoutSubHandler(_logger.Object);
        var withCustomRunner = new BotPreCheckoutSubHandler(
            _logger.Object,
            new PayChainRunner<BotPreCheckoutSubHandler, PreCheckoutQuery>());

        withDefaultRunner.Should().NotBeNull();
        withCustomRunner.Should().NotBeNull();
    }
}
