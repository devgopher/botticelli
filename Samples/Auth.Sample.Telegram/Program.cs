using Auth.Sample.Telegram.Commands;
using Auth.Sample.Telegram.Commands.Processors;
using Botticelli.Auth.Data.Postgres;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Framework.Extensions;
using Botticelli.Framework.Telegram.Extensions;
using NLog.Extensions.Logging;
using Telegram.Bot.Types.ReplyMarkups;

var builder = WebApplication.CreateBuilder(args);

builder.Services
       .AddTelegramBot(builder.Configuration)
       .AddTelegramLayoutsSupport()
       .AddLogging(cfg => cfg.AddNLog())
       .AddPostgresBasicBotUserAuth(builder.Configuration);

builder.Services.AddBotCommand<StartCommand>()
       .AddProcessor<StartCommandProcessor<ReplyKeyboardMarkup>>()
       .AddValidator<PassValidator<StartCommand>>();

builder.Services.AddBotCommand<InfoCommand>()
       .AddProcessor<InfoCommandProcessor<ReplyKeyboardMarkup>>()
       .AddValidator<PassValidator<InfoCommand>>();

builder.Build().Run();