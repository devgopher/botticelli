using Botticelli.Interfaces;
using Botticelli.Scheduler.Hangfire;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Mocks;
using NUnit.Framework;

namespace Botticelli.Scheduler.Hangfire.Tests;

[TestFixture]
public class ContainerJobActivatorTests
{
    [Test]
    public void ActivateJob_Should_ResolveMatchingIBotService()
    {
        var services = new ServiceCollection();
        services.AddSingleton<IBot<BotMock>, BotMock>();
        var activator = new ContainerJobActivator(services);

        var resolved = activator.ActivateJob(typeof(BotMock));

        resolved.Should().BeOfType<BotMock>();
    }

    [Test]
    public void ActivateJob_Should_Throw_WhenNoMatchingService()
    {
        var services = new ServiceCollection();
        var activator = new ContainerJobActivator(services);

        var act = () => activator.ActivateJob(typeof(BotMock));

        act.Should().Throw<ArgumentNullException>();
    }
}
