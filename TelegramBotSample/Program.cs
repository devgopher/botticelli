using Botticelli.Framework.Extensions;
using Botticelli.Framework.Options;
using TelegramBotSample;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTelegramBot(new BotOptionsBuilder<TelegramBotSettings>()
    .Set(s => s.ChatPollingIntervalMs = 500)
    .Set(s => s.TelegramToken = "5746549361:AAFZcvuRcEk7QO4OfAjTYQQUeUpcaES3kqk")
    .Set(s => s.Name = "test_bot"));

builder.Services.AddHostedService<TestBotHostedService>();

var app = builder.Build();

app.Run();

