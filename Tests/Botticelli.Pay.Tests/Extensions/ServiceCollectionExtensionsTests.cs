using Botticelli.Pay.Extensions;
using Botticelli.Pay.Handlers;
using Botticelli.Pay.Processors;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Botticelli.Pay.Tests.Extensions;

[TestFixture]
public class ServiceCollectionExtensionsTests
{
    private sealed class TestHandler : IPreCheckoutHandler;

    private sealed class TestProcessor : DummyPayProcessor<TestHandler, string>;

    [Test]
    public void AddPayPreCheckout_Should_RegisterBuilder()
    {
        var services = new ServiceCollection();

        var builder = services.AddPayPreCheckout<TestHandler, TestProcessor, string>();

        builder.Should().NotBeNull();
        services.BuildServiceProvider()
                .GetRequiredService<PayChainBuilder<TestHandler, TestProcessor, string>>()
                .Should().NotBeNull();
    }

    [Test]
    public void AddPayPreCheckout_WithProcessor_Should_AddElement()
    {
        var services = new ServiceCollection();
        var processor = new TestProcessor();

        var builder = services.AddPayPreCheckout<TestHandler, TestProcessor, string>(processor);
        var runner = builder.Build();

        runner.Should().NotBeNull();
    }

    [Test]
    public void AddPayPreCheckouts_WithParams_Should_AddAll()
    {
        var services = new ServiceCollection();

        var builder = services.AddPayPreCheckouts<TestHandler, TestProcessor, string>(
            new TestProcessor(),
            new TestProcessor());

        var runner = builder.Build();
        runner.Should().NotBeNull();
    }

    [Test]
    public void AddPaySuccessful_And_AddPayError_Should_RegisterBuilders()
    {
        var services = new ServiceCollection();

        var successful = services.AddPaySuccessful<TestHandler, TestProcessor, string>();
        var error = services.AddPayError<TestHandler, TestProcessor, string>();

        successful.Should().NotBeNull();
        error.Should().NotBeNull();
    }

    [Test]
    public async Task AddPayments_Should_RegisterRunner()
    {
        var services = new ServiceCollection();

        var runner = services.AddPayments<TestHandler, TestProcessor, string>();
        await using var provider = services.BuildServiceProvider();

        var resolved = provider.GetRequiredService<PayChainRunner<TestHandler, string>>();
        var result = await resolved.Run("x", CancellationToken.None);

        runner.Should().BeSameAs(resolved);
        result.isSuccessful.Should().BeTrue();
    }
}
