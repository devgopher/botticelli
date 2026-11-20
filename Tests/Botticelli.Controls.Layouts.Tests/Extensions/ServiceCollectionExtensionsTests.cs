using Botticelli.Controls.Layouts.CommandProcessors.InlineCalendar;
using Botticelli.Controls.Layouts.Commands.InlineCalendar;
using Botticelli.Controls.Layouts.Extensions;
using Botticelli.Controls.Layouts.Inlines;
using Botticelli.Controls.Parsers;
using Botticelli.Framework.Commands.Processors;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Shared.ValueObjects;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Telegram.Bot.Types.ReplyMarkups;

namespace Botticelli.Controls.Layouts.Tests.Extensions;

[TestFixture]
public class ServiceCollectionExtensionsTests
{
    [Test]
    public void AddInlineCalendar_Should_RegisterExpectedServiceDescriptors()
    {
        var services = new ServiceCollection();
        services.AddInlineCalendar<InlineKeyboardMarkup, FakeLayoutSupplier, FakeDateChosenProcessor>();

        services.Should().Contain(d => d.ServiceType == typeof(ILayoutSupplier<InlineKeyboardMarkup>) &&
                                       d.ImplementationType == typeof(FakeLayoutSupplier));
        services.Should().Contain(d => d.ServiceType == typeof(ICCommandProcessor<MonthForwardCommand, InlineKeyboardMarkup>));
        services.Should().Contain(d => d.ServiceType == typeof(ICCommandProcessor<MonthBackwardCommand, InlineKeyboardMarkup>));
        services.Should().Contain(d => d.ServiceType == typeof(ICCommandProcessor<YearForwardCommand, InlineKeyboardMarkup>));
        services.Should().Contain(d => d.ServiceType == typeof(ICCommandProcessor<YearBackwardCommand, InlineKeyboardMarkup>));
        services.Should().Contain(d => d.ServiceType == typeof(FakeDateChosenProcessor));
        services.Should().Contain(d => d.ServiceType == typeof(ICommandValidator<DateChosenCommand>) &&
                                       d.ImplementationType == typeof(PassValidator<DateChosenCommand>));
        services.Should().Contain(d => d.ServiceType == typeof(ICommandValidator<YearForwardCommand>));
        services.Should().Contain(d => d.ServiceType == typeof(ICommandValidator<YearBackwardCommand>));
        services.Should().Contain(d => d.ServiceType == typeof(ICommandValidator<MonthForwardCommand>));
        services.Should().Contain(d => d.ServiceType == typeof(ICommandValidator<MonthBackwardCommand>));
    }

    private sealed class FakeLayoutSupplier : ILayoutSupplier<InlineKeyboardMarkup>
    {
        public InlineKeyboardMarkup GetMarkup(ILayout layout) => new(new List<InlineKeyboardButton>());
    }

    private sealed class FakeDateChosenProcessor : CommandProcessor<DateChosenCommand>
    {
        public FakeDateChosenProcessor()
            : base(
                Microsoft.Extensions.Logging.Abstractions.NullLogger<FakeDateChosenProcessor>.Instance,
                new PassValidator<DateChosenCommand>(),
                new Shared.Validation.MessageValidator())
        {
        }

        protected override Task InnerProcess(Message message, CancellationToken token) => Task.CompletedTask;
    }
}
