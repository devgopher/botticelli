using BotDataSecureStorage.Settings;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Framework.Extensions;
using Botticelli.Framework.Options;
using Botticelli.Framework.Vk.Messages;
using Botticelli.Framework.Vk.Messages.Extensions;
using Botticelli.Framework.Vk.Messages.Options;
using Botticelli.Schedule.Hangfire.Extensions;
using MessagingSample.Common.Commands;
using MessagingSample.Common.Commands.Processors;
using NLog.Extensions.Logging;
using VkMessagingSample;
using VkMessagingSample.Settings;

var builder = WebApplication.CreateBuilder(args);

var settings = builder.Configuration
    .GetSection(nameof(SampleSettings))
    .Get<SampleSettings>();
var vkSettings = builder.Configuration
    .GetSection(nameof(VkBotSettings))
    .Get<VkBotSettings>();

builder.Services
    .Configure<SampleSettings>(builder.Configuration.GetSection(nameof(SampleSettings)))
    .AddVkBot(builder.Configuration,
        new BotOptionsBuilder<VkBotSettings>()
            .Set(s => s.SecureStorageSettings = new SecureStorageSettings
            {
                ConnectionString = settings.SecureStorageConnectionString
            })
            .Set(s =>
            {
                s.Name = settings.BotName;
                s.PollIntervalMs = 100;
                s.GroupId = vkSettings.GroupId;
            }))
    .AddLogging(cfg => cfg.AddNLog())
    .AddHangfireScheduler(builder.Configuration)
    .AddHostedService<TestBotHostedService>()
    .AddScoped<StartCommandProcessor>()
    .AddScoped<StopCommandProcessor>()
    .AddBotCommand<StartCommand, StartCommandProcessor, PassValidator<StartCommand>>()
    .AddBotCommand<StopCommand, StopCommandProcessor, PassValidator<StopCommand>>();

var app = builder.Build();
app.Services.RegisterBotCommand<StartCommand, StartCommandProcessor, VkBot>();
app.Services.RegisterBotCommand<StopCommand, StopCommandProcessor, VkBot>();

app.Run();