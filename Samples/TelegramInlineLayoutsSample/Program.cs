using Botticelli.Bot.Interfaces.Processors;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Framework.Controls.Layouts.Extensions;
using Botticelli.Framework.Extensions;
using Botticelli.Framework.Options;
using Botticelli.Framework.Telegram;
using Botticelli.Framework.Telegram.Decorators;
using Botticelli.Framework.Telegram.Extensions;
using Botticelli.Framework.Telegram.Layout;
using Botticelli.Framework.Telegram.Options;
using Botticelli.Locations.Telegram.Extensions;
using Botticelli.SecureStorage.Settings;
using NLog.Extensions.Logging;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramInlineLayoutsSample.Commands;
using TelegramInlineLayoutsSample.Handlers;
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
                                        .Set(s => s.Timeout = 20000)
                                        .Set(s => s.Name = "test_bot")
                                ,
                                TelegramClientDecoratorBuilder.Instance(builder.Services)
                                                              .AddThrottler(new Throttler()))
       .AddLogging(cfg => cfg.AddNLog())
       .AddBotCommand<GetCalendarCommand, GetCalendarCommandProcessor, PassValidator<GetCalendarCommand>>()
       .AddInlineCalendar<InlineKeyboardMarkup, InlineTelegramLayoutSupplier, DateChosenCommandProcessor>()
       .AddOsmLocations(builder.Configuration);

var app = builder.Build();
app.Services.UseInlineCalendar<TelegramBot, DateChosenCommandProcessor>()
   .RegisterBotCommand<GetCalendarCommand, GetCalendarCommandProcessor, TelegramBot>()
   .RegisterOsmLocationsCommands();

app.Run();