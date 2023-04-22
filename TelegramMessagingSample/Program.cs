using BotDataSecureStorage.Settings;
using Botticelli.BotBase.Extensions;
using Botticelli.Framework.Options;
using Botticelli.Framework.Telegram;
using Botticelli.Framework.Telegram.Extensions;
using Botticelli.Framework.Telegram.Options;
using Botticelli.Interfaces;
using NLog.Extensions.Logging;
using TelegramBotSample;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTelegramBot(new BotOptionsBuilder<TelegramBotSettings>()
                                .Set(s => s.ChatPollingIntervalMs = 20000)
                                .Set(s => s.SecureStorageSettings = new SecureStorageSettings
                                {
                                    ConnectionString = "Filename=../../../database.db;Password=123;ReadOnly=true"
                                })
                                .Set(s => s.Name = "test_messaging_bot"));
builder.Services.UseBotticelli<IBot<TelegramBot>>(builder.Configuration);
builder.Services.AddLogging(cfg => cfg.AddNLog());

builder.Services.AddHostedService<TestBotHostedService>();

var app = builder.Build();

app.Run();

