using BotDataSecureStorage.Settings;
using Botticelli.Framework.Options;
using Botticelli.Framework.Telegram;
using Botticelli.Framework.Telegram.Extensions;
using Botticelli.Framework.Telegram.Options;
using Botticelli.Scheduler.Extensions;
using NLog.Extensions.Logging;
using TelegramMessagingSample;
using TelegramMessagingSample.Settings;

var builder = WebApplication.CreateBuilder(args);

var settings = builder.Configuration
                      .GetSection(nameof(SampleSettings))
                      .Get<SampleSettings>();

builder.Services
       .Configure<SampleSettings>(builder.Configuration.GetSection(nameof(SampleSettings)))
       .AddTelegramBot(builder.Configuration,
                       new BotOptionsBuilder<TelegramBotSettings>()
                               .Set(s => s.SecureStorageSettings = new SecureStorageSettings
                               {
                                   ConnectionString = settings.SecureStorageConnectionString
                               })
                               .Set(s => s.Name = settings.BotName))
       .AddLogging(cfg => cfg.AddNLog())
       .AddHangfireScheduler<TelegramBot>(builder.Configuration)
       .AddHostedService<TestBotHostedService>();

builder.Build().Run();