using Botticelli.Framework.Commands.Validators;
using Botticelli.Framework.Controls.Layouts.CommandProcessors.InlineCalendar;
using Botticelli.Framework.Controls.Layouts.Commands.InlineCalendar;
using Botticelli.Framework.Controls.Parsers;
using Botticelli.Framework.Extensions;
using Botticelli.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types.ReplyMarkups;

namespace Botticelli.Framework.Controls.Layouts.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInlineCalendar<TReplyMarkup, TLayoutSupplier>(this IServiceCollection services)
            where TLayoutSupplier : class, ILayoutSupplier<TReplyMarkup>
        => services.AddScoped<ILayoutSupplier<TReplyMarkup>, TLayoutSupplier>()
                   .AddScoped<ICCommandProcessor<MonthForwardCommand, InlineKeyboardMarkup>>()
                   .AddScoped<ICCommandProcessor<MonthBackwardCommand, InlineKeyboardMarkup>>()
                   .AddScoped<ICCommandProcessor<YearForwardCommand, InlineKeyboardMarkup>>()
                   .AddScoped<ICCommandProcessor<YearBackwardCommand, InlineKeyboardMarkup>>()
                   .AddScoped<ICommandValidator<YearForwardCommand>, PassValidator<YearForwardCommand>>()
                   .AddScoped<ICommandValidator<YearBackwardCommand>, PassValidator<YearBackwardCommand>>()
                   .AddScoped<ICommandValidator<MonthForwardCommand>, PassValidator<MonthForwardCommand>>()
                   .AddScoped<ICommandValidator<MonthBackwardCommand>, PassValidator<MonthBackwardCommand>>();

    public static IServiceProvider UseInlineCalendar<TBot>(this IServiceProvider sp)
            where TBot : IBot<TBot> =>
            sp.RegisterBotCommand<MonthForwardCommand, ICCommandProcessor<MonthForwardCommand, InlineKeyboardMarkup>, TBot>()
              .RegisterBotCommand<MonthBackwardCommand, ICCommandProcessor<MonthBackwardCommand, InlineKeyboardMarkup>, TBot>()
              .RegisterBotCommand<YearForwardCommand, ICCommandProcessor<YearForwardCommand, InlineKeyboardMarkup>, TBot>()
              .RegisterBotCommand<YearBackwardCommand, ICCommandProcessor<YearBackwardCommand, InlineKeyboardMarkup>, TBot>();
}