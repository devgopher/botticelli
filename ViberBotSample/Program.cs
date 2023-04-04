using BotDataSecureStorage.Settings;
using Botticelli.AI.Extensions;
using Botticelli.BotBase.Extensions;
using Botticelli.Bus.Rabbit.Extensions;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Framework.Extensions;
using Botticelli.Framework.Options;
using Botticelli.Framework.Viber;
using Botticelli.Framework.Viber.Extensions;
using Botticelli.Framework.Viber.Options;
using Botticelli.Interfaces;
using Botticelli.Talks.Extensions;
using NLog.Extensions.Logging;
using ViberBotSample;
using ViberBotSample.Commands;
using ViberBotSample.Handlers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddViberBot(new BotOptionsBuilder<ViberBotSettings>()
                                .Set(s => s.WebHookUrl = "https://botticellibots.com/")
                                .Set(s => s.SecureStorageSettings = new SecureStorageSettings
                                {
                                    ConnectionString = "Filename=../../../database.db;Password=123;ReadOnly=true"
                                })
                                .Set(s => s.Name = "test_bot"));

builder.Services.UseBotticelli<IBot<ViberBot>>(builder.Configuration);
builder.Services.AddLogging(cfg => cfg.AddNLog());
builder.Services.AddOpenTtsTalks(builder.Configuration);
builder.Services.AddGptJProvider(builder.Configuration);
builder.Services.AddScoped<ICommandValidator<SampleCommand>, PassValidator<SampleCommand>>();
builder.Services.AddScoped<ICommandValidator<AiCommand>, PassValidator<AiCommand>>();
builder.Services.AddSingleton<AiHandler>();
builder.Services.UseRabbitBusAgent<IBot<ViberBot>, AiHandler>(builder.Configuration);
builder.Services.UseRabbitBusClient<IBot<ViberBot>>(builder.Configuration);

builder.Services.AddHostedService<TestBotHostedService>();
builder.Services.AddBotCommand<SampleCommand, SampleCommandProcessor, PassValidator<SampleCommand>>();
builder.Services.AddBotCommand<AiCommand, AiCommandProcessor, PassValidator<AiCommand>>();

var app = builder.Build();
app.Services.RegisterBotCommand<SampleCommand, SampleCommandProcessor, ViberBot>()
   .RegisterBotCommand<AiCommand, AiCommandProcessor, ViberBot>();
app.Run();
