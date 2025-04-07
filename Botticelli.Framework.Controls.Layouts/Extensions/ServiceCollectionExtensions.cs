using Botticelli.Framework.Commands.Processors;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Framework.Controls.Layouts.CommandProcessors.InlineCalendar;
using Botticelli.Framework.Controls.Layouts.Commands.InlineCalendar;
using Botticelli.Framework.Controls.Parsers;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types.ReplyMarkups;

namespace Botticelli.Framework.Controls.Layouts.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInlineCalendar<TReplyMarkup, TLayoutSupplier, TDateChosenCommandProcessor>(this IServiceCollection services)
            where TLayoutSupplier : class, ILayoutSupplier<TReplyMarkup>
            where TDateChosenCommandProcessor : CommandProcessor<DateChosenCommand>
    {
        return services.AddSingleton<ILayoutSupplier<TReplyMarkup>, TLayoutSupplier>()
                       .AddSingleton<ICCommandProcessor<MonthForwardCommand, InlineKeyboardMarkup>>()
                       .AddSingleton<ICCommandProcessor<MonthBackwardCommand, InlineKeyboardMarkup>>()
                       .AddSingleton<ICCommandProcessor<YearForwardCommand, InlineKeyboardMarkup>>()
                       .AddSingleton<ICCommandProcessor<YearBackwardCommand, InlineKeyboardMarkup>>()
                       .AddSingleton<TDateChosenCommandProcessor>()
                       .AddSingleton<ICommandValidator<YearForwardCommand>, PassValidator<YearForwardCommand>>()
                       .AddSingleton<ICommandValidator<YearBackwardCommand>, PassValidator<YearBackwardCommand>>()
                       .AddSingleton<ICommandValidator<MonthForwardCommand>, PassValidator<MonthForwardCommand>>()
                       .AddSingleton<ICommandValidator<MonthBackwardCommand>, PassValidator<MonthBackwardCommand>>()
                       .AddSingleton<ICommandValidator<DateChosenCommand>, PassValidator<DateChosenCommand>>();
    }
}