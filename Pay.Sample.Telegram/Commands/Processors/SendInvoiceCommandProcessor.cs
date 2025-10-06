using Botticelli.Client.Analytics;
using Botticelli.Framework.Commands.Processors;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Pay.Message;
using Botticelli.Pay.Models;
using Botticelli.Pay.Utils;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.ValueObjects;
using FluentValidation;
using Microsoft.Extensions.Options;
using TelegramPayBot.Settings;

namespace TelegramPayBot.Commands.Processors;

public class SendInvoiceCommandProcessor<TReplyMarkup> : CommandProcessor<SendInvoiceCommand> where TReplyMarkup : class
{
    private readonly IOptionsMonitor<PaySettings> _paySettingsAccessor;

    public SendInvoiceCommandProcessor(ILogger<SendInvoiceCommandProcessor<TReplyMarkup>> logger,
                                       ICommandValidator<SendInvoiceCommand> commandValidator,
                                       MetricsProcessor metricsProcessor,
                                       IValidator<Message> messageValidator,
                                       IOptionsMonitor<PaySettings> paySettingsAccessor)
            : base(logger,
                   commandValidator,
                   messageValidator,
                   metricsProcessor)
    {
        _paySettingsAccessor = paySettingsAccessor;
    }

    protected override Task InnerProcessContact(Message message, CancellationToken token)
    {
        return Task.CompletedTask;
    }

    protected override Task InnerProcessPoll(Message message, CancellationToken token)
    {
        return Task.CompletedTask;
    }

    protected override Task InnerProcessLocation(Message message, CancellationToken token)
    {
        return Task.CompletedTask;
    }

    protected override async Task InnerProcess(Message message, CancellationToken token)
    {
        var sendInvoiceMessageRequest = new SendMessageRequest
        {
            Message = new PayInvoiceMessage
            {
                Uid = Guid.NewGuid().ToString(),
                ChatIds = message.ChatIds,
                Body = "This is a test payments bot.\nEnjoy!",
                Invoice = new Invoice
                {
                    Title = "Test invoice (no real payment will be made)",
                    Currency = CurrencySelector.SelectCurrency("RUB"),
                    Description = "Test invoice",
                    Payload = "Test payload",
                    Prices =
                    [
                        new Price
                        {
                            Label = "Item 1",
                            Amount = 120
                        },
                        new Price
                        {
                            Label = "Item 2",
                            Amount = 150
                        }
                    ],
                    ProviderToken = _paySettingsAccessor.CurrentValue.ProviderToken
                }
            }
        };

        await SendMessage(sendInvoiceMessageRequest, token);
    }
}