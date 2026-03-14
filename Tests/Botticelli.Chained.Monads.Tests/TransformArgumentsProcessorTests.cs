using System.Threading;
using System.Threading.Tasks;
using Botticelli.Chained.Context;
using Botticelli.Chained.Monads.Commands;
using Botticelli.Chained.Monads.Commands.Processors;
using Botticelli.Chained.Monads.Commands.Result;
using FluentAssertions;
using LanguageExt;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Botticelli.Chained.Monads.Tests;

[TestFixture]
public class TransformArgumentsProcessorTests
{
    private TransformArgumentsProcessor<TestCommand, string> _processor = null!;
    private Mock<ILogger<TransformArgumentsProcessor<TestCommand, string>>> _loggerMock = null!;

    [SetUp]
    public void SetUp()
    {
        _loggerMock = new Mock<ILogger<TransformArgumentsProcessor<TestCommand, string>>>();
        _processor = new TransformArgumentsProcessor<TestCommand, string>(_loggerMock.Object);
    }

    [Test]
    public async Task Process_ShouldApplySuccessFunc_OnSuccessResult()
    {
        var command = TestCommand.CreateWithArgs("hello");
        var success = SuccessResult<TestCommand>.Create(command);
        var either = EitherAsync<FailResult<TestCommand>, SuccessResult<TestCommand>>.Right(success);

        _processor.SuccessFunc = s => s.ToUpperInvariant();

        var result = await _processor.Process(either, CancellationToken.None);
        var final = await result.ToEither();

        final.IsRight.Should().BeTrue();
        command.Context.Get<string>(Names.Args).Should().Be("HELLO");
    }

    [Test]
    public async Task Process_ShouldApplyFailFunc_OnFailResult()
    {
        var command = TestCommand.CreateWithArgs("world");
        var fail = FailResult<TestCommand>.Create(command, "error");
        var either = EitherAsync<FailResult<TestCommand>, SuccessResult<TestCommand>>.Left(fail);

        _processor.FailFunc = s => s + "!";

        var result = await _processor.Process(either, CancellationToken.None);
        var final = await result.ToEither();

        final.IsLeft.Should().BeTrue();
        command.Context.Get<string>(Names.Args).Should().Be("world!");
    }

    [Test]
    public async Task Process_ShouldLogAndApplyFailFunc_OnException()
    {
        var command = TestCommand.CreateWithArgs("boom");
        var success = SuccessResult<TestCommand>.Create(command);
        var either = EitherAsync<FailResult<TestCommand>, SuccessResult<TestCommand>>.Right(success);

        _processor.SuccessFunc = _ => throw new System.Exception("fail");
        _processor.FailFunc = s => s + " handled";

        var result = await _processor.Process(either, CancellationToken.None);
        var final = await result.ToEither();

        final.IsLeft.Should().BeTrue();
        command.Context.Get<string>(Names.Args).Should().Be("boom handled");
    }

    private sealed record TestCommand(ICommandContext Context) : IChainCommand
    {
        public static TestCommand CreateWithArgs(string args)
        {
            var storage = new InMemoryStorage<string, string>();
            var context = new CommandContext(storage);
            context.Set(Names.Args, args);
            return new TestCommand(context);
        }
    }
}

