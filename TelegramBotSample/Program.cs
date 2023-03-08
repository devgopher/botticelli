using BotDataSecureStorage.Settings;
using Botticelli.AI.Extensions;
using Botticelli.BotBase.Extensions;
using Botticelli.Bus.Rabbit.Extensions;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Framework.Extensions;
using Botticelli.Framework.Options;
using Botticelli.Framework.Telegram;
using Botticelli.Framework.Telegram.Extensions;
using Botticelli.Framework.Telegram.Options;
using Botticelli.Interfaces;
using Botticelli.Talks.Extensions;
using NLog.Extensions.Logging;
using TelegramBotSample;
using TelegramBotSample.Commands;
using TelegramBotSample.Handlers;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddTelegramBot(new BotOptionsBuilder<TelegramBotSettings>()
                                .Set(s => s.ChatPollingIntervalMs = 20000)
                                .Set(s => s.SecureStorageSettings = new SecureStorageSettings
                                {
                                    ConnectionString = "Filename=../../../database.db;Password=123;ReadOnly=true"
                                })
                                .Set(s => s.Name = "test_bot"));

builder.Services.UseBotticelli<IBot<TelegramBot>>(builder.Configuration);
builder.Services.AddLogging(cfg => cfg.AddNLog());
builder.Services.AddOpenTtsTalks(builder.Configuration);
builder.Services.AddGptJProvider(builder.Configuration);
builder.Services.AddScoped<ICommandValidator<SampleCommand>, PassValidator<SampleCommand>>();
builder.Services.AddScoped<ICommandValidator<AiCommand>, PassValidator<AiCommand>>();
builder.Services.AddSingleton<AiHandler>();
builder.Services.UseRabbitBusAgent<IBot<TelegramBot>, AiHandler>(builder.Configuration);
builder.Services.UseRabbitBusClient<IBot<TelegramBot>>(builder.Configuration);

//builder.Services.UsePassBusAgent<IBot<TelegramBot>, AiHandler>();
//builder.Services.UsePassBusClient<IBot<TelegramBot>>();

builder.Services.AddHostedService<TestBotHostedService>();
builder.Services.AddBotCommand<SampleCommand, SampleCommandProcessor, PassValidator<SampleCommand>>();
builder.Services.AddBotCommand<AiCommand, AiCommandProcessor, PassValidator<AiCommand>>();

var app = builder.Build();
app.Services.RegisterBotCommand<SampleCommand, SampleCommandProcessor, IBot<TelegramBot>>()
   .RegisterBotCommand<AiCommand, AiCommandProcessor, IBot<TelegramBot>>();

app.Run();