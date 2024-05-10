using Botticelli.Framework.Commands.Validators;
using Botticelli.Framework.Controls.Layouts.CommandProcessors.InlineCalendar;
using Botticelli.Framework.Controls.Layouts.Commands.InlineCalendar;
using Botticelli.Framework.Controls.Layouts.Extensions;
using Botticelli.Framework.Controls.Layouts.Inlines;
using Botticelli.Framework.Extensions;
using Botticelli.Framework.Options;
using Botticelli.Framework.Telegram;
using Botticelli.Framework.Telegram.Extensions;
using Botticelli.Framework.Telegram.Layout;
using Botticelli.Framework.Telegram.Options;
using Botticelli.Interfaces;
using Botticelli.SecureStorage.Settings;
using NLog.Extensions.Logging;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramInlineLayoutsSample.Settings;

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
    .AddInlineCalendar<InlineKeyboardMarkup, InlineTelegramLayoutSupplier>();

var app = builder.Build();
app.Services.UseInlineCalendar<TelegramBot>();

app.Run();