using Botticelli.Controls.Parsers;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Framework.Extensions;
using Botticelli.Interfaces;
using Botticelli.Pay.Models;
using Botticelli.Pay.Processors;
using Botticelli.Pay.Telegram.Extensions;
using NLog.Extensions.Logging;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramPayBot.Commands;
using TelegramPayBot.Commands.Processors;
using TelegramPayBot.Handlers;
using TelegramPayBot.Settings;

var builder = WebApplication.CreateBuilder(args);

var bot = builder.Services
       .Configure<PaySettings>(builder.Configuration.GetSection("PaySettings"))
       .AddTelegramPayBot<PayPreCheckoutHandler, DummyPayProcessor<PayPreCheckoutHandler, PreCheckoutQuery>>(builder.Configuration)
       .Build();

builder.Services.AddLogging(cfg => cfg.AddNLog())
       .AddSingleton<ILayoutParser, JsonLayoutParser>()
       .AddSingleton<IBot>(bot);

builder.Services.AddBotCommand<InfoCommand>()
       .AddProcessor<InfoCommandProcessor<ReplyKeyboardMarkup>>()
       .AddValidator<PassValidator<InfoCommand>>();

builder.Services.AddBotCommand<SendInvoiceCommand>()
       .AddProcessor<SendInvoiceCommandProcessor<ReplyKeyboardMarkup>>()
       .AddValidator<PassValidator<SendInvoiceCommand>>();

var app = builder.Build();

await app.RunAsync();