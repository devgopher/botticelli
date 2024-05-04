using AiSample.Common;
using AiSample.Common.Commands;
using AiSample.Common.Handlers;
using AiSample.Common.Settings;
using Botticelli.AI.DeepSeekGpt.Extensions;
using Botticelli.AI.Extensions;
using Botticelli.Bus.None.Extensions;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Framework.Controls.Extensions;
using Botticelli.Framework.Extensions;
using Botticelli.Framework.Options;
using Botticelli.Framework.Telegram;
using Botticelli.Framework.Telegram.Extensions;
using Botticelli.Framework.Telegram.Options;
using Botticelli.Interfaces;
using Botticelli.SecureStorage.Settings;
using NLog.Extensions.Logging;
using Telegram.Bot.Types.ReplyMarkups;

var builder = WebApplication.CreateBuilder(args);

var settings = builder.Configuration
    .GetSection(nameof(SampleSettings))
    .Get<SampleSettings>();

builder.Services.AddTelegramBot(builder.Configuration,
        new BotOptionsBuilder<TelegramBotSettings>()
            .Set(s => s.SecureStorageSettings = new SecureStorageSettings
            {
                ConnectionString = settings?.SecureStorageConnectionString
            })
            .Set(s => s.Name = "test_bot"))
    .AddLogging(cfg => cfg.AddNLog())
    .AddDeepSeekProvider(builder.Configuration)
    .AddAiValidation()
    .AddScoped<ICommandValidator<AiCommand>, PassValidator<AiCommand>>()
    .AddSingleton<AiHandler>()
    .UsePassBusAgent<IBot<TelegramBot>, AiHandler>()
    .UsePassBusClient<IBot<TelegramBot>>()
    .UsePassEventBusClient<IBot<TelegramBot>>()
    .AddBotCommand<AiCommand, AiCommandProcessor<ReplyMarkupBase>, PassValidator<AiCommand>>()
    .AddTelegramLayoutsSupport();

var app = builder.Build();
app.Services.RegisterBotCommand<AiCommand, AiCommandProcessor<ReplyMarkupBase>, TelegramBot>();

app.Run();