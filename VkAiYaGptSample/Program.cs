using AiSample.Common;
using AiSample.Common.Commands;
using AiSample.Common.Handlers;
using AiSample.Common.Settings;
using BotDataSecureStorage.Settings;
using Botticelli.AI.YaGpt.Extensions;
using Botticelli.Bus.None.Extensions;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Framework.Extensions;
using Botticelli.Framework.Options;
using Botticelli.Framework.Vk.Messages;
using Botticelli.Framework.Vk.Messages.Extensions;
using Botticelli.Framework.Vk.Messages.Options;
using Botticelli.Interfaces;
using NLog.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

var settings = builder.Configuration
    .GetSection(nameof(SampleSettings))
    .Get<SampleSettings>();

builder.Services.AddVkBot(builder.Configuration,
        new BotOptionsBuilder<VkBotSettings>()
            .Set(s => s.SecureStorageSettings = new SecureStorageSettings
            {
                ConnectionString = settings.SecureStorageConnectionString
            })
            .Set(s => s.Name = "test_bot"))
    .AddLogging(cfg => cfg.AddNLog())
    .AddYaGptProvider(builder.Configuration)
    .AddScoped<ICommandValidator<AiCommand>, PassValidator<AiCommand>>()
    .AddSingleton<AiHandler>()
    .UsePassBusAgent<IBot<VkBot>, AiHandler>()
    .UsePassBusClient<IBot<VkBot>>()
    .AddBotCommand<AiCommand, AiCommandProcessor, PassValidator<AiCommand>>();

var app = builder.Build();
app.Services.RegisterBotCommand<AiCommand, AiCommandProcessor, VkBot>();

app.Run();