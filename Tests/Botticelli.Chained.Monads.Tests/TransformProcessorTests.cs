using System;
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

namespace Botticelli.Chained.Monads.Tests;

[TestFixture]
public class TransformProcessorTests
{
    private TransformProcessor<TestCommand> _processor = null!;
    private Mock<ILogger<TransformProcessor<TestCommand>>> _loggerMock = null!;

    [SetUp]
    public void SetUp()
    {
        _loggerMock = new Mock<ILogger<TransformProcessor<TestCommand>>>();
        _processor = new TransformProcessor<TestCommand>(_loggerMock.Object);
    }

    [Test]
    public async Task Process_ShouldApplySuccessFunc_OnSuccessResult()
    {
        var command = TestCommand.Create();
        var input = SuccessResult<TestCommand>.Create(command);
        var either = EitherAsync<FailResult<TestCommand>, SuccessResult<TestCommand>>.Right(input);

        _processor.SuccessFunc = r =>
        {
            r.Should().BeSameAs(input);
            return r;
        };

        var result = await _processor.Process(either, CancellationToken.None);
        var final = await result.ToEither();

        final.IsRight.Should().BeTrue();
        final.IfRight(r => r.Should().BeSameAs(input));
    }

    [Test]
    public async Task Process_ShouldApplyFailFunc_OnFailResult()
    {
        var command = TestCommand.Create();
        var fail = FailResult<TestCommand>.Create(command, "error");
        var either = EitherAsync<FailResult<TestCommand>, SuccessResult<TestCommand>>.Left(fail);

        var called = false;
        _processor.FailFunc = f =>
        {
            called = true;
            f.Should().BeSameAs(fail);
            return f;
        };

        var result = await _processor.Process(either, CancellationToken.None);
        var final = await result.ToEither();

        called.Should().BeTrue();
        final.IsLeft.Should().BeTrue();
        final.IfLeft(l => l.Should().BeSameAs(fail));
    }

    [Test]
    public async Task Process_ShouldFallbackToFailFunc_WhenSuccessFuncThrows()
    {
        var command = TestCommand.Create();
        var input = SuccessResult<TestCommand>.Create(command);
        var either = EitherAsync<FailResult<TestCommand>, SuccessResult<TestCommand>>.Right(input);

        _processor.SuccessFunc = _ => throw new InvalidOperationException("boom");

        var mapped = FailResult<TestCommand>.Create(command, "mapped");
        _processor.FailFunc = _ => mapped;

        var result = await _processor.Process(either, CancellationToken.None);
        var final = await result.ToEither();

        final.IsLeft.Should().BeTrue();
        final.IfLeft(l => l.Should().Be(mapped));
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
}

