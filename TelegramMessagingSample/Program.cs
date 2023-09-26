using BotDataSecureStorage.Settings;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Framework.Extensions;
using Botticelli.Framework.Options;
using Botticelli.Framework.Telegram;
using Botticelli.Framework.Telegram.Extensions;
using Botticelli.Framework.Telegram.Options;
using Botticelli.Interfaces;
using Botticelli.Scheduler.Extensions;
using MessagingSample.Common.Commands;
using MessagingSample.Common.Commands.Processors;
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
       .AddHangfireScheduler(builder.Configuration)
       .AddHostedService<TestBotHostedService>()
       .AddScoped<StartCommandProcessor>()
       .AddScoped<StopCommandProcessor>()
       .AddBotCommand<StartCommand, StartCommandProcessor, PassValidator<StartCommand>>()
       .AddBotCommand<StopCommand, StopCommandProcessor, PassValidator<StopCommand>>();

var app = builder.Build();
app.Services.RegisterBotCommand<StartCommand, StartCommandProcessor, TelegramBot>();
app.Services.RegisterBotCommand<StopCommand, StopCommandProcessor, TelegramBot>();

app.Run();