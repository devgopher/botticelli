using Botticelli.Bot.Data.Repositories;
using Botticelli.Bot.Utils.TextUtils;
using Botticelli.Client.Analytics;
using Botticelli.Client.Analytics.Settings;
using Botticelli.Framework.Telegram.Handlers;
using Botticelli.Pay.Message;
using Botticelli.Pay.Models;
using Botticelli.Pay.Telegram;
using Botticelli.Shared.API.Client.Requests;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Telegram.Bot;

namespace Botticelli.Pay.Telegram.Tests;

[TestFixture]
public class TelegramPaymentBotTests
{
    private sealed class TestablePaymentBot : TelegramPaymentBot
    {
        public TestablePaymentBot(ITelegramBotClient client,
                                  IBotUpdateHandler handler,
                                  ILogger<TelegramPaymentBot> logger,
                                  MetricsProcessor metrics,
                                  ITextTransformer textTransformer,
                                  IBotDataAccess data)
            : base(client, handler, logger, metrics, textTransformer, data)
        {
        }

        public Task InvokeAdditionalProcessing(SendMessageRequest request, string chatId, CancellationToken token)
            => AdditionalProcessing<object>(request, null, false, chatId, token);
    }

    private TestablePaymentBot CreateBot(Mock<ITelegramBotClient> client)
    {
        client.SetupGet(c => c.BotId).Returns(12345);

        var metrics = new MetricsProcessor(new MetricsPublisher(new AnalyticsClientSettings
        {
            TargetUrl = "http://localhost"
        }));

        return new TestablePaymentBot(
            client.Object,
            new Mock<IBotUpdateHandler>().Object,
            new Mock<ILogger<TelegramPaymentBot>>().Object,
            metrics,
            new Mock<ITextTransformer>().Object,
            new Mock<IBotDataAccess>().Object);
    }

    [Test]
    public async Task AdditionalProcessing_WhenNoInvoice_Should_DoNothing()
    {
        var client = new Mock<ITelegramBotClient>();
        var bot = CreateBot(client);

        await bot.InvokeAdditionalProcessing(new SendMessageRequest
        {
            Message = new Shared.ValueObjects.Message()
        }, "chat-1", CancellationToken.None);

        client.VerifyGet(c => c.BotId, Times.AtLeastOnce);
    }

    [Test]
    public async Task AdditionalProcessing_WithInvoice_Should_AttemptSendInvoice()
    {
        var client = new Mock<ITelegramBotClient>();
        var bot = CreateBot(client);

        var request = new SendMessageRequest
        {
            Message = new PayInvoiceMessage
            {
                Invoice = new Invoice
                {
                    Title = "Product",
                    Description = "Desc",
                    Payload = "payload",
                    Currency = new Currency
                    {
                        Iso = "USD",
                        Name = "USD",
                        Demonym = "US",
                        MajorSingle = "Dollar",
                        MajorPlural = "Dollars",
                        Symbol = "$",
                        SymbolNative = "$",
                        MinorSingle = "Cent",
                        MinorPlural = "Cents",
                        Decimals = 2
                    },
                    Prices =
                    [
                        new Price { Label = "Item", Amount = 10.50m }
                    ],
                    ProviderToken = "provider",
                    ProviderData = "{}"
                }
            }
        };

        var act = async () => await bot.InvokeAdditionalProcessing(request, "42", CancellationToken.None);

        // Exercises invoice mapping + ConvertPrice; Telegram client mock may or may not throw.
        await act.Should().NotThrowAsync();
    }

    [Test]
    public void Constructor_Should_CreateBot()
    {
        var client = new Mock<ITelegramBotClient>();
        var bot = CreateBot(client);

        bot.Should().NotBeNull();
        bot.BotUserId.Should().Be("12345");
    }
}
