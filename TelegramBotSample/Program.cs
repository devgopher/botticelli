using Botticelli.BotBase.Extensions;
using Botticelli.Framework.Options;
using Botticelli.Framework.Telegram;
using Botticelli.Framework.Telegram.Extensions;
using Botticelli.Framework.Telegram.Options;
using Botticelli.Interfaces;
using Botticelli.Talks.Extensions;
using NLog.Extensions.Logging;
using TelegramBotSample;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddTelegramBot(new BotOptionsBuilder<TelegramBotSettings>()
                                .Set(s => s.ChatPollingIntervalMs = 20000)
                                .Set(s => s.TelegramToken = "5746549361:AAFZcvuRcEk7QO4OfAjTYQQUeUpcaES3kqk")
                                .Set(s => s.Name = "test_bot"));

builder.Services.UseBotticelli<IBot<TelegramBot>>(builder.Configuration);
builder.Services.AddLogging(cfg => cfg.AddNLog());
builder.Services.AddOpenTtsTalks(builder.Configuration);


builder.Services.AddHostedService<TestBotHostedService>();

var app = builder.Build();

app.Run();