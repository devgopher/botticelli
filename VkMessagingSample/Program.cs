using BotDataSecureStorage.Settings;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Framework.Extensions;
using Botticelli.Framework.Options;
using Botticelli.Framework.Vk;
using Botticelli.Framework.Vk.Messages;
using Botticelli.Framework.Vk.Messages.Extensions;
using Botticelli.Framework.Vk.Messages.Options;
using Botticelli.Scheduler.Extensions;
using NLog.Extensions.Logging;
using VkMessagingSample;
using VkMessagingSample.Commands;
using VkMessagingSample.Settings;

var builder = WebApplication.CreateBuilder(args);

var settings = builder.Configuration
                      .GetSection(nameof(SampleSettings))
                      .Get<SampleSettings>();

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
                             s.GroupId = 221973506;
                         }))
       .AddLogging(cfg => cfg.AddNLog())
       .AddHangfireScheduler<VkBot>(builder.Configuration)
       .AddHostedService<TestBotHostedService>()
       .AddScoped<StartCommandProcessor>()
       .AddBotCommand<StartCommand, StartCommandProcessor, PassValidator<StartCommand>>();

var app = builder.Build();
app.Services.RegisterBotCommand<StartCommand, StartCommandProcessor, VkBot>();

app.Run();