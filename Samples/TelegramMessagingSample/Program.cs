using Botticelli.Framework.Commands.Validators;
using Botticelli.Framework.Controls.Parsers;
using Botticelli.Framework.Extensions;
using Botticelli.Framework.Telegram;
using Botticelli.Framework.Telegram.Extensions;
using Botticelli.Schedule.Quartz.Extensions;
using MessagingSample.Common.Commands;
using MessagingSample.Common.Commands.Processors;
using NLog.Extensions.Logging;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramMessagingSample;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddTelegramBot(builder.Configuration)
    .AddLogging(cfg => cfg.AddNLog())
    .AddQuartzScheduler(builder.Configuration)
    .AddHostedService<TestBotHostedService>()
    .AddScoped<ILayoutParser, JsonLayoutParser>();

builder.Services.AddBotCommand<InfoCommand>()
    .AddProcessor<InfoCommandProcessor<ReplyMarkupBase>>()
    .AddValidator<PassValidator<InfoCommand>>();

builder.Services.AddBotCommand<StartCommand>()
    .AddProcessor<StartCommandProcessor<ReplyMarkupBase>>()
    .AddValidator<PassValidator<StartCommand>>();

builder.Services.AddBotCommand<StopCommand>()
    .AddProcessor<StopCommandProcessor<ReplyMarkupBase>>()
    .AddValidator<PassValidator<StopCommand>>();

builder.Services.AddEndpointsApiExplorer()
    .AddSwaggerGen();

var app = builder.Build();
app.Services.RegisterBotCommand<StartCommand, StartCommandProcessor<ReplyMarkupBase>, TelegramBot>()
    .RegisterProcessor<StopCommandProcessor<ReplyMarkupBase>>()
    .RegisterProcessor<InfoCommandProcessor<ReplyMarkupBase>>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();