using Botticelli.Framework.Commands.Validators;
using Botticelli.Framework.Controls.Layouts.Extensions;
using Botticelli.Framework.Extensions;
using Botticelli.Framework.Telegram;
using Botticelli.Framework.Telegram.Extensions;
using Botticelli.Framework.Telegram.Layout;
using Botticelli.Locations.Telegram.Extensions;
using NLog.Extensions.Logging;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramInlineLayoutsSample.Commands;
using TelegramInlineLayoutsSample.Handlers;
using TelegramInlineLayoutsSample.Settings;

var builder = WebApplication.CreateBuilder(args);

var settings = builder.Configuration
                      .GetSection(nameof(SampleSettings))
                      .Get<SampleSettings>();

builder.Services
       .AddTelegramBot(optionsBuilder => optionsBuilder.Set(s => s.SecureStorageConnectionString = settings.SecureStorageConnectionString)
                                                       .Set(s => s.Name = settings?.BotName))
       .AddLogging(cfg => cfg.AddNLog())
       .AddBotCommand<GetCalendarCommand, GetCalendarCommandProcessor, PassValidator<GetCalendarCommand>>()
       .AddInlineCalendar<InlineKeyboardMarkup, InlineTelegramLayoutSupplier, DateChosenCommandProcessor>()
       .AddOsmLocations(builder.Configuration);

var app = builder.Build();
app.Services.UseInlineCalendar<TelegramBot, DateChosenCommandProcessor>()
   .RegisterBotCommand<GetCalendarCommand, GetCalendarCommandProcessor, TelegramBot>()
   .RegisterOsmLocationsCommands();

app.Run();