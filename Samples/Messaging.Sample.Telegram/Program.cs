using Botticelli.Broadcasting.Extensions;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Framework.Extensions;
using Botticelli.Framework.Telegram.Extensions;
using Botticelli.Schedule.Quartz.Extensions;
using MessagingSample.Common.Commands;
using MessagingSample.Common.Commands.Processors;
using Microsoft.EntityFrameworkCore;
using NLog.Extensions.Logging;
using Telegram.Bot.Types.ReplyMarkups;

var builder = WebApplication.CreateBuilder(args);

builder.Services
       .AddTelegramBot(builder.Configuration, botBuilder => botBuilder.AddBroadcasting(builder.Configuration,
              optionsBuilder => optionsBuilder.UseSqlite()))
       .AddTelegramLayoutsSupport()
       .AddLogging(cfg => cfg.AddNLog())
       .AddQuartzScheduler(builder.Configuration);

builder.Services.AddBotCommand<InfoCommand>()
       .AddProcessor<InfoCommandProcessor<ReplyKeyboardMarkup>>()
       .AddValidator<PassValidator<InfoCommand>>();

builder.Services.AddBotCommand<StartCommand>()
       .AddProcessor<StartCommandProcessor<ReplyKeyboardMarkup>>()
       .AddValidator<PassValidator<StartCommand>>();

builder.Services.AddBotCommand<StopCommand>()
       .AddProcessor<StopCommandProcessor<ReplyKeyboardMarkup>>()
       .AddValidator<PassValidator<StopCommand>>();

await builder.Build().RunAsync();