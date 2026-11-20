using Botticelli.Pay.Processors;
using Botticelli.Pay.Telegram.Handlers;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Payments;
using TgMessage = Telegram.Bot.Types.Message;

namespace Botticelli.Pay.Telegram.Tests.Handlers;

[TestFixture]
public class BotSuccessfulPaymentSubHandlerTests
{
    [Test]
    public async Task Process_WhenNoSuccessfulPayment_Should_Return()
    {
        var logger = new Mock<ILogger<BotSuccessfulPaymentSubHandler>>();
        var client = new Mock<ITelegramBotClient>();
        var handler = new BotSuccessfulPaymentSubHandler(logger.Object);

        await handler.Process(client.Object, new Update { Message = new TgMessage() }, CancellationToken.None);

        client.VerifyNoOtherCalls();
    }

    [Test]
    public async Task Process_WhenSuccessfulPaymentPresent_Should_Return()
    {
        var logger = new Mock<ILogger<BotSuccessfulPaymentSubHandler>>();
        var client = new Mock<ITelegramBotClient>();
        var runner = new PayChainRunner<BotSuccessfulPaymentSubHandler, Botticelli.Pay.Models.PreCheckoutQuery>();
        var handler = new BotSuccessfulPaymentSubHandler(logger.Object, runner);

        var update = new Update
        {
            Message = new TgMessage
            {
                SuccessfulPayment = new SuccessfulPayment
                {
                    Currency = "USD",
                    TotalAmount = 100,
                    InvoicePayload = "p",
                    TelegramPaymentChargeId = "t",
                    ProviderPaymentChargeId = "p"
                }
            }
        };

        await handler.Process(client.Object, update, CancellationToken.None);
    }

    [Test]
    public void Constructors_Should_CreateHandler()
    {
        var logger = new Mock<ILogger<BotSuccessfulPaymentSubHandler>>();

        new BotSuccessfulPaymentSubHandler(logger.Object).Should().NotBeNull();
        new BotSuccessfulPaymentSubHandler(
                logger.Object,
                new PayChainRunner<BotSuccessfulPaymentSubHandler, Botticelli.Pay.Models.PreCheckoutQuery>())
            .Should().NotBeNull();
    }
}
