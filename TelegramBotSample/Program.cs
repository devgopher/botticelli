using BotDataSecureStorage.Settings;
using Botticelli.AI.Extensions;
using Botticelli.Bot.Interfaces.Handlers;
using Botticelli.BotBase.Extensions;
using Botticelli.Bus.None.Extensions;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Framework.Options;
using Botticelli.Framework.Telegram;
using Botticelli.Framework.Telegram.Extensions;
using Botticelli.Framework.Telegram.Options;
using Botticelli.Interfaces;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;
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
builder.Services.AddScoped<IHandler<SendMessageRequest, SendMessageResponse>, AiHandler>();
builder.Services.RegisterBotCommand<SampleCommand, SampleCommandProcessor>();
builder.Services.RegisterBotCommand<AiCommand, AiCommandProcessor>();
builder.Services.AddScoped<ICommandValidator<SampleCommand>, PassValidator<SampleCommand>>();
builder.Services.AddScoped<ICommandValidator<AiCommand>, PassValidator<AiCommand>>();
builder.Services.UsePassBusAgent<IBot<TelegramBot>, AiHandler>();
builder.Services.UsePassBusClient<IBot<TelegramBot>>();
builder.Services.AddHostedService<TestBotHostedService>();

var app = builder.Build();

app.Run();