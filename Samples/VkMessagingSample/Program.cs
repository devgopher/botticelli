using Botticelli.Framework.Commands.Validators;
using Botticelli.Framework.Controls.Parsers;
using Botticelli.Framework.Extensions;
using Botticelli.Framework.Options;
using Botticelli.Framework.Vk.Messages;
using Botticelli.Framework.Vk.Messages.API.Markups;
using Botticelli.Framework.Vk.Messages.Extensions;
using Botticelli.Framework.Vk.Messages.Options;
using Botticelli.Schedule.Quartz.Extensions;
using Botticelli.Talks.Extensions;
using MessagingSample.Common.Commands;
using MessagingSample.Common.Commands.Processors;
using NLog.Extensions.Logging;
using VkMessagingSample;
using VkMessagingSample.Settings;

var builder = WebApplication.CreateBuilder(args);

var settings = builder.Configuration
                      .GetSection(nameof(SampleSettings))
                      .Get<SampleSettings>();

builder.Services
       .Configure<SampleSettings>(builder.Configuration.GetSection(nameof(SampleSettings)))
       .AddVkBot(builder.Configuration,
                 new BotOptionsBuilder<VkBotSettings>()
                         .Set(s => s.SecureStorageConnectionString = settings.SecureStorageConnectionString)
                         .Set(s => s.Name = settings?.BotName))
       .AddLogging(cfg => cfg.AddNLog())
       .AddQuartzScheduler(builder.Configuration)
       .AddHostedService<TestBotHostedService>()
       .AddScoped<StartCommandProcessor<VkKeyboardMarkup>>()
       .AddScoped<StopCommandProcessor<VkKeyboardMarkup>>()
       .AddOpenTtsTalks(builder.Configuration)
       .AddScoped<ILayoutParser, JsonLayoutParser>()
       .AddBotCommand<StartCommand, StartCommandProcessor<VkKeyboardMarkup>, PassValidator<StartCommand>>()
       .AddBotCommand<StopCommand, StopCommandProcessor<VkKeyboardMarkup>, PassValidator<StopCommand>>();


var app = builder.Build();
app.Services.RegisterBotCommand<StartCommand, StartCommandProcessor<VkKeyboardMarkup>, VkBot>();
app.Services.RegisterBotCommand<StopCommand, StopCommandProcessor<VkKeyboardMarkup>, VkBot>();

app.Run();