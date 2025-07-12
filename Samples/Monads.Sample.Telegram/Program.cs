using Botticelli.Chained.Context.Redis.Extensions;
using Botticelli.Chained.Monads.Commands.Processors;
using Botticelli.Chained.Monads.Extensions;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Framework.Extensions;
using Botticelli.Framework.Telegram.Extensions;
using Botticelli.Framework.Telegram.Layout;
using Botticelli.Interfaces;
using NLog.Extensions.Logging;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramMonadsBasedBot.Commands;

var builder = WebApplication.CreateBuilder(args);

var bot = builder.Services
                 .AddTelegramBot(builder.Configuration)
                 .Build();

builder.Services       
       .AddLogging(cfg => cfg.AddNLog())
       .AddTelegramLayoutsSupport()
       .AddSingleton<IBot>(bot);

builder.Services
    .AddChainedRedisStorage<string, string>(builder.Configuration)
    .AddBotCommand<MathCommand>()
    .AddMonadsChain<MathCommand, PassValidator<MathCommand>, ReplyKeyboardMarkup, ReplyTelegramLayoutSupplier>(builder.Services,
                                                                                                                  cb => cb.Next<InputCommandProcessor<MathCommand>>()
                                                                                                                          .Next<TransformArgumentsProcessor<MathCommand, double>>(tp => tp.SuccessFunc =
                                                                                                                                                                                          Math.Sqrt)
                                                                                                                          .Next<TransformArgumentsProcessor<MathCommand, double>>(tp => tp.SuccessFunc =
                                                                                                                                                                                          Math.Sqrt)
                                                                                                                          .Next<TransformArgumentsProcessor<MathCommand, double>>(tp => tp.SuccessFunc =
                                                                                                                                                                                          Math.Cos)
                                                                                                                          .Next<TransformArgumentsProcessor<MathCommand, double>>(tp => tp.SuccessFunc =
                                                                                                                                                                                          Math.Abs)
                                                                                                                          .Next<TransformArgumentsProcessor<MathCommand, double>>(tp => tp.SuccessFunc =
                                                                                                                                                                                          Math.Sqrt)
                                                                                                                          .Next<OutputCommandProcessor<ReplyKeyboardMarkup, MathCommand>>());

var app = builder.Build();

await app.RunAsync();