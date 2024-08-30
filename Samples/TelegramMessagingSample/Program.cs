using Botticelli.Framework.Commands.Validators;
using Botticelli.Framework.Controls.Parsers;
using Botticelli.Framework.Extensions;
using Botticelli.Framework.Options;
using Botticelli.Framework.Telegram;
using Botticelli.Framework.Telegram.Decorators;
using Botticelli.Framework.Telegram.Extensions;
using Botticelli.Framework.Telegram.Options;
using Botticelli.LoadTests.Receiver.Controller;
using Botticelli.Schedule.Quartz.Extensions;
using Botticelli.SecureStorage.Settings;
using Botticelli.Talks.Extensions;
using MessagingSample.Common.Commands;
using MessagingSample.Common.Commands.Processors;
using NLog.Extensions.Logging;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramMessagingSample;
using TelegramMessagingSample.Settings;
using Botticelli.LoadTests.Receiver.Extensions;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

var builder = WebApplication.CreateBuilder(args);

var settings = builder.Configuration
    .GetSection(nameof(SampleSettings))
    .Get<SampleSettings>();


builder.Services
       .Configure<SampleSettings>(builder.Configuration.GetSection(nameof(SampleSettings)))
       .AddTelegramBot(builder.Configuration,
                       new BotOptionsBuilder<TelegramBotSettings>()
                               .Set(s => s.SecureStorageSettings = new SecureStorageSettings
                               {
                                   ConnectionString = settings?.SecureStorageConnectionString
                               })
                               .Set(s => s.Name = settings?.BotName),
                       TelegramClientDecoratorBuilder.Instance(builder.Services))
       .AddLogging(cfg => cfg.AddNLog())
       .AddQuartzScheduler(builder.Configuration)
       .AddHostedService<TestBotHostedService>()
       .AddScoped<StartCommandProcessor<ReplyMarkupBase>>()
       .AddScoped<StopCommandProcessor<ReplyMarkupBase>>()
       .AddScoped<InfoCommandProcessor<ReplyMarkupBase>>()
       .AddOpenTtsTalks(builder.Configuration)
       .AddScoped<ILayoutParser, JsonLayoutParser>()
       .AddBotCommand<InfoCommand, InfoCommandProcessor<ReplyMarkupBase>, PassValidator<InfoCommand>>()
       .AddBotCommand<StartCommand, StartCommandProcessor<ReplyMarkupBase>, PassValidator<StartCommand>>()
       .AddBotCommand<StopCommand, StopCommandProcessor<ReplyMarkupBase>, PassValidator<StopCommand>>();

       builder.AddLoadTesting<TelegramBot>();

builder.Services.AddEndpointsApiExplorer()
       .AddSwaggerGen();

var app = builder.Build();
app.Services.RegisterBotCommand<StartCommand, StartCommandProcessor<ReplyMarkupBase>, TelegramBot>()
            .RegisterBotCommand<StopCommand, StopCommandProcessor<ReplyMarkupBase>, TelegramBot>()
            .RegisterBotCommand<InfoCommand, InfoCommandProcessor<ReplyMarkupBase>, TelegramBot>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();