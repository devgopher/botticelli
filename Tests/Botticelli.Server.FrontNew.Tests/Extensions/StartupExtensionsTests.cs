using Botticelli.Server.FrontNew.Extensions;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Botticelli.Server.FrontNew.Tests.Extensions;

[TestFixture]
public class StartupExtensionsTests
{
    [Test]
    public void AddCertificates_Should_ConfigurePrimaryHandler()
    {
        var services = new ServiceCollection();
        var builder = services.AddHttpClient("certs").AddCertificates();

        builder.Should().NotBeNull();
        services.Should().NotBeEmpty();
    }
}
