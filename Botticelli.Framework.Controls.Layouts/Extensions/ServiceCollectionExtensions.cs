using Botticelli.Framework.Commands.Processors;
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
    public static IServiceCollection AddInlineCalendar<TReplyMarkup, TLayoutSupplier, TDateChosenCommandProcessor>(this IServiceCollection services ,ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where TLayoutSupplier : class, ILayoutSupplier<TReplyMarkup>
            where TDateChosenCommandProcessor : CommandProcessor<DateChosenCommand>
        => services.Add<ILayoutSupplier<TReplyMarkup>, TLayoutSupplier>(lifetime)
                   .Add<ICCommandProcessor<MonthForwardCommand, InlineKeyboardMarkup>>(lifetime)
                   .Add<ICCommandProcessor<MonthBackwardCommand, InlineKeyboardMarkup>>(lifetime)
                   .Add<ICCommandProcessor<YearForwardCommand, InlineKeyboardMarkup>>(lifetime)
                   .Add<ICCommandProcessor<YearBackwardCommand, InlineKeyboardMarkup>>(lifetime)
                   .Add<TDateChosenCommandProcessor>(lifetime)
                   .Add<ICommandValidator<YearForwardCommand>, PassValidator<YearForwardCommand>>(lifetime)
                   .Add<ICommandValidator<YearBackwardCommand>, PassValidator<YearBackwardCommand>>(lifetime)
                   .Add<ICommandValidator<MonthForwardCommand>, PassValidator<MonthForwardCommand>>(lifetime)
                   .Add<ICommandValidator<MonthBackwardCommand>, PassValidator<MonthBackwardCommand>>(lifetime)
                   .Add<ICommandValidator<DateChosenCommand>, PassValidator<DateChosenCommand>>(lifetime);

    public static IServiceProvider UseInlineCalendar<TBot, TDateChosenCommandProcessor>(this IServiceProvider sp)
            where TDateChosenCommandProcessor : CommandProcessor<DateChosenCommand>
            where TBot : IBot<TBot> =>
            sp.RegisterBotCommand<ICCommandProcessor<MonthForwardCommand, InlineKeyboardMarkup>, TBot>()
              .RegisterBotCommand<ICCommandProcessor<MonthBackwardCommand, InlineKeyboardMarkup>, TBot>()
              .RegisterBotCommand<ICCommandProcessor<YearForwardCommand, InlineKeyboardMarkup>, TBot>()
              .RegisterBotCommand<ICCommandProcessor<YearBackwardCommand, InlineKeyboardMarkup>, TBot>()
              .RegisterBotCommand<TDateChosenCommandProcessor, TBot>();
   
}