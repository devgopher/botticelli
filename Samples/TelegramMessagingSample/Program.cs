using Botticelli.Framework.Commands.Validators;
using Botticelli.Framework.Controls.Parsers;
using Botticelli.Framework.Extensions;
using Botticelli.Framework.Options;
using Botticelli.Framework.Telegram;
using Botticelli.Framework.Telegram.Extensions;
using Botticelli.Schedule.Quartz.Extensions;
using MessagingSample.Common.Commands;
using MessagingSample.Common.Commands.Processors;
using NLog.Extensions.Logging;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramMessagingSample;
using TelegramMessagingSample.Settings;

var builder = WebApplication.CreateBuilder(args);

var settings = builder.Configuration
                      .GetSection(nameof(SampleSettings))
                      .Get<SampleSettings>();

var serverSettings = builder.Configuration
                      .GetSection(nameof(ServerSettings))
                      .Get<ServerSettings>();

builder.Services
       .Configure<SampleSettings>(builder.Configuration.GetSection(nameof(SampleSettings)))
       .AddTelegramBot(o => o.Set(s => s.SecureStorageConnectionString = settings.SecureStorageConnectionString)
                                                       .Set(s => s.Name = settings?.BotName),
                       o => o.Set(s => s.TargetUrl = "https://"),
                       o => o.Set(s => s.ServerUri = serverSettings.ServerUri),
                       o => o.Set(s => s.ConnectionString = settings.SecureStorageConnectionString))
       .AddLogging(cfg => cfg.AddNLog())
       .AddQuartzScheduler(builder.Configuration)
       .AddHostedService<TestBotHostedService>()
       .AddScoped<StartCommandProcessor<ReplyMarkupBase>>()
       .AddScoped<StopCommandProcessor<ReplyMarkupBase>>()
       .AddScoped<InfoCommandProcessor<ReplyMarkupBase>>()
       .AddScoped<ILayoutParser, JsonLayoutParser>()
       .AddBotCommand<InfoCommand, InfoCommandProcessor<ReplyMarkupBase>, PassValidator<InfoCommand>>()
       .AddBotCommand<StartCommand, StartCommandProcessor<ReplyMarkupBase>, PassValidator<StartCommand>>()
       .AddBotCommand<StopCommand, StopCommandProcessor<ReplyMarkupBase>, PassValidator<StopCommand>>();

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