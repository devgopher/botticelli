using BotDataSecureStorage.Settings;
using Botticelli.AI.Extensions;
using Botticelli.Bus.None.Extensions;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Framework.Extensions;
using Botticelli.Framework.Options;
using Botticelli.Framework.Telegram;
using Botticelli.Framework.Telegram.Extensions;
using Botticelli.Framework.Telegram.Options;
using Botticelli.Interfaces;
using NLog.Extensions.Logging;
using TelegramAiChatGptSample;
using TelegramAiChatGptSample.Commands;
using TelegramAiChatGptSample.Handlers;
using TelegramAiChatGptSample.Settings;

var builder = WebApplication.CreateBuilder(args);

var settings = builder.Configuration
                      .GetSection(nameof(SampleSettings))
                      .Get<SampleSettings>();

builder.Services.AddTelegramBot(builder.Configuration,
                                new BotOptionsBuilder<TelegramBotSettings>()
                                        .Set(s => s.SecureStorageSettings = new SecureStorageSettings
                                        {
                                            ConnectionString = settings.SecureStorageConnectionString
                                        })
                                        .Set(s => s.Name = "test_bot"))
       .AddLogging(cfg => cfg.AddNLog())
       .AddChatGptProvider(builder.Configuration)
       .AddScoped<ICommandValidator<AiCommand>, PassValidator<AiCommand>>()
       .AddSingleton<AiHandler>()
       .UsePassBusAgent<IBot<TelegramBot>, AiHandler>()
       .UsePassBusClient<IBot<TelegramBot>>()
       .AddBotCommand<AiCommand, AiCommandProcessor, PassValidator<AiCommand>>();

var app = builder.Build();
app.Services.RegisterBotCommand<AiCommand, AiCommandProcessor, TelegramBot>();

app.Run();