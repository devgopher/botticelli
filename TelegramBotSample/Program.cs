using Botticelli.BotBase.Extensions;
using Botticelli.Framework.Options;
using Botticelli.Framework.Telegram.Extensions;
using Botticelli.Framework.Telegram.Options;
using Botticelli.Framework.Viber.Extensions;
using Botticelli.Framework.Viber.Options;
using TelegramBotSample;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTelegramBot(new BotOptionsBuilder<TelegramBotSettings>()
    .Set(s => s.ChatPollingIntervalMs = 500)
    .Set(s => s.TelegramToken = "5746549361:AAFZcvuRcEk7QO4OfAjTYQQUeUpcaES3kqk")
    .Set(s => s.Name = "test_bot"));

//builder.Services.AddViberBot(new BotOptionsBuilder<ViberBotSettings>()
//    .Set(s => s.ViberToken = "5065bdf5c527dfe8-3dfad317d974d1-ac5916e258fc1a93")
//    .Set(s => s.Name = "test_bot"));

builder.Services.AddHostedService<TestBotHostedService>();
builder.Services.UseBotticelli(builder.Configuration);

var app = builder.Build();

app.Run();

