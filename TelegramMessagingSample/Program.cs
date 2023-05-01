using BotDataSecureStorage.Settings;
using Botticelli.Framework.Options;
using Botticelli.Framework.Telegram;
using Botticelli.Framework.Telegram.Extensions;
using Botticelli.Framework.Telegram.Options;
using Botticelli.Interfaces;
using Botticelli.Scheduler.Extensions;
using NLog.Extensions.Logging;
using TelegramBotSample;
using TelegramMessagingSample.Settings;

var builder = WebApplication.CreateBuilder(args);

var sampleSettings = new SampleSettings();
builder.Configuration.GetSection(nameof(SampleSettings)).Bind(sampleSettings);

builder.Services
       .AddTelegramBot(builder.Configuration,
                       new BotOptionsBuilder<TelegramBotSettings>()
                               .Set(s => s.SecureStorageSettings = new SecureStorageSettings
                               {
                                   ConnectionString = sampleSettings.SecureStorageConnectionString
                               })
                               .Set(s => s.Name = "test_messaging_bot"))
       .AddLogging(cfg => cfg.AddNLog())
       .AddSingleton(sampleSettings)
       .AddHangfireScheduler<TelegramBot>(builder.Configuration)
       .AddHostedService<TestBotHostedService>();

var app = builder.Build();

app.Run();