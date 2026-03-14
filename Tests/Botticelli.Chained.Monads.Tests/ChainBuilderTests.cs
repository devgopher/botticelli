using System.Linq;
using Botticelli.Chained.Monads.Commands;
using Botticelli.Chained.Monads.Commands.Processors;
using Botticelli.Chained.Monads.Commands.Result;
using Botticelli.Chained.Context;
using Botticelli.Bot.Interfaces;
using FluentAssertions;
using LanguageExt;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace Botticelli.Chained.Monads.Tests;

[TestFixture]
public class ChainBuilderTests
{
    [Test]
    public void Build_ShouldThrow_WhenBotIsNotConfigured()
    {
        var services = new ServiceCollection();
        services.AddLogging();

        var builder = new ChainBuilder<TestCommand>(services)
            .Next(new NoopProcessor());

        FluentActions.Invoking(() => builder.Build())
                     .Should().Throw<NullReferenceException>()
                     .WithMessage("*Bot should be set up*");
    }

    [Test]
    public void Build_ShouldSetBotForAllProcessors_WhenBotProvided()
    {
        var services = new ServiceCollection();
        services.AddLogging();

        var botMock = new Mock<IBot>();

        var first = new NoopProcessor();
        var second = new NoopProcessor();

        var builder = new ChainBuilder<TestCommand>(services)
            .SetBot(botMock.Object)
            .Next(first)
            .Next(second);

        var runner = builder.Build();

        runner.Should().NotBeNull();
        first.Bot.Should().BeSameAs(botMock.Object);
        second.Bot.Should().BeSameAs(botMock.Object);
    }

    private sealed record TestCommand(ICommandContext Context) : IChainCommand
    {
        public static TestCommand Create()
        {
            var storage = new InMemoryStorage<string, string>();
            var context = new CommandContext(storage);
            return new TestCommand(context);
        }
    }

    private sealed class NoopProcessor : IChainProcessor<TestCommand>
    {
        public IBot Bot { get; private set; } = null!;

        public void SetBot(IBot bot) => Bot = bot;

        public Task<EitherAsync<FailResult<TestCommand>, SuccessResult<TestCommand>>> Process(
            EitherAsync<FailResult<TestCommand>, SuccessResult<TestCommand>> stepResult,
            CancellationToken token = default) => Task.FromResult(stepResult);
    }
}

