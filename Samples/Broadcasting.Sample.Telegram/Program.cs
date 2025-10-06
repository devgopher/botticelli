using Botticelli.Broadcasting.Telegram.Extensions;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Framework.Extensions;
using Botticelli.Framework.Telegram.Extensions;
using Broadcasting.Sample.Common.Commands;
using Broadcasting.Sample.Common.Commands.Processors;
using Telegram.Bot.Types.ReplyMarkups;

var builder = WebApplication.CreateBuilder(args);

builder.Services
       .AddTelegramBot(builder.Configuration)
       .AddTelegramBroadcasting(builder.Configuration)
       .Prepare();

builder.Services.AddBotCommand<StartCommand>()
       .AddProcessor<StartCommandProcessor<ReplyKeyboardMarkup>>()
       .AddValidator<PassValidator<StartCommand>>();

var app = builder.Build();
app.Urls.Clear();       
app.Urls.Add("http://localhost:5044");

await app.RunAsync();