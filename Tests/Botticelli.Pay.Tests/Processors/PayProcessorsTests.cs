using Botticelli.Pay.Handlers;
using Botticelli.Pay.Models;
using Botticelli.Pay.Processors;
using FluentAssertions;
using NUnit.Framework;

namespace Botticelli.Pay.Tests.Processors;

[TestFixture]
public class PayProcessorsTests
{
    private sealed class TestHandler : IPreCheckoutHandler;

    private sealed class FailingProcessor : IPayProcessor<TestHandler, string>
    {
        public Task<(bool isSuccess, string errorMessage)> Process(string request, CancellationToken token)
            => Task.FromResult((false, $"fail:{request}"));
    }

    private sealed class CountingProcessor : IPayProcessor<TestHandler, string>
    {
        public int Calls { get; private set; }

        public Task<(bool isSuccess, string errorMessage)> Process(string request, CancellationToken token)
        {
            Calls++;
            return Task.FromResult((true, string.Empty));
        }
    }

    private sealed class ConfigurableProcessor : DummyPayProcessor<TestHandler, string>
    {
        public string Tag { get; set; } = "default";
    }

    [Test]
    public async Task DummyPayProcessor_Should_AlwaysSucceed()
    {
        var processor = new DummyPayProcessor<TestHandler, PreCheckoutQuery>();

        var result = await processor.Process(new PreCheckoutQuery
        {
            Id = "1",
            From = new Shared.ValueObjects.User { Id = "u" },
            Currency = new Currency
            {
                Name = "n",
                Demonym = "d",
                MajorSingle = "m",
                MajorPlural = "mp",
                Symbol = "s",
                SymbolNative = "sn",
                MinorSingle = "mi",
                MinorPlural = "mip"
            },
            TotalAmount = 1,
            InvoicePayload = "p"
        }, CancellationToken.None);

        result.isSuccess.Should().BeTrue();
        result.errorMessage.Should().BeEmpty();
    }

    [Test]
    public async Task PayChainRunner_Empty_Should_Succeed()
    {
        var runner = new PayChainRunner<TestHandler, string>();

        var result = await runner.Run("x", CancellationToken.None);

        result.isSuccessful.Should().BeTrue();
        result.errorMessage.Should().BeEmpty();
    }

    [Test]
    public async Task PayChainRunner_Should_StopOnFirstFailure()
    {
        var second = new CountingProcessor();
        var runner = new PayChainRunner<TestHandler, string>(
        [
            new FailingProcessor(),
            second
        ]);

        var result = await runner.Run("payload", CancellationToken.None);

        result.isSuccessful.Should().BeFalse();
        result.errorMessage.Should().Be("fail:payload");
        second.Calls.Should().Be(0);
    }

    [Test]
    public async Task PayChainBuilder_Should_BuildAndRunProcessors()
    {
        var builder = new PayChainBuilder<TestHandler, ConfigurableProcessor, string>();
        builder.AddElement<ConfigurableProcessor>(p =>
        {
            p.Tag = "first";
            return p;
        });
        builder.AddElement(new ConfigurableProcessor { Tag = "second" });

        var runner = builder.Build();
        var result = await runner.Run("ok", CancellationToken.None);

        result.isSuccessful.Should().BeTrue();
    }

    [Test]
    public async Task PayChainRunner_AllSuccess_Should_ReturnLastSuccess()
    {
        var runner = new PayChainRunner<TestHandler, string>(
        [
            new CountingProcessor(),
            new CountingProcessor()
        ]);

        var result = await runner.Run("ok", CancellationToken.None);

        result.isSuccessful.Should().BeTrue();
    }
}
