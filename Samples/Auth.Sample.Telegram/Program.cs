using Auth.Sample.Telegram.Commands;
using Auth.Sample.Telegram.Commands.Processors;
using Botticelli.Auth.Data.Sqlite;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Framework.Extensions;
using Botticelli.Framework.Telegram.Extensions;
using Botticelli.Interfaces;
using NLog.Extensions.Logging;
using Telegram.Bot.Types.ReplyMarkups;

var builder = WebApplication.CreateBuilder(args);

var bot = builder.Services
                 .AddTelegramBot(builder.Configuration)
                 .Build();

builder.Services
       .AddTelegramLayoutsSupport()
       .AddLogging(cfg => cfg.AddNLog())
       .AddSqliteBasicBotUserAuth(builder.Configuration)
       .AddSingleton<IBot>(bot);

builder.Services.AddBotCommand<StartCommand>()
       .AddProcessor<StartCommandProcessor<ReplyKeyboardMarkup>>()
       .AddValidator<PassValidator<StartCommand>>();

builder.Services.AddBotCommand<RegisterCommand>()
       .AddProcessor<RegisterCommandProcessor<ReplyKeyboardMarkup>>()
       .AddValidator<PassValidator<RegisterCommand>>();

builder.Services.AddBotCommand<InfoCommand>()
       .AddProcessor<InfoCommandProcessor<ReplyKeyboardMarkup>>()
       .AddValidator<PassValidator<InfoCommand>>();

await builder.Build().RunAsync();