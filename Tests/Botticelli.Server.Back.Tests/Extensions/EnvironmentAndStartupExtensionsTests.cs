using Botticelli.Server.Back.Extensions;
using Botticelli.Server.Back.Services.Broadcasting;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Botticelli.Server.Back.Tests.Extensions;

[TestFixture]
public class EnvironmentAndStartupExtensionsTests
{
    [Test]
    public void IsDevelopment_Should_RespectEnvironmentVariable()
    {
        var previous = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        try
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
            EnvironmentExtensions.IsDevelopment().Should().BeTrue();

            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Production");
            EnvironmentExtensions.IsDevelopment().Should().BeFalse();
        }
        finally
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", previous);
        }
    }

    [Test]
    public void AddBroadcasting_Should_RegisterService()
    {
        var services = new ServiceCollection();
        services.AddBroadcasting();

        services.Should().Contain(d => d.ServiceType == typeof(IBroadcastService) &&
                                       d.ImplementationType == typeof(BroadcastService));
    }

    [Test]
    public void AddIdentity_Should_ConfigureOptions()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddIdentity();
        services.AddOptions();

        using var provider = services.BuildServiceProvider();
        var identity = provider.GetRequiredService<Microsoft.Extensions.Options.IOptions<IdentityOptions>>().Value;

        identity.Password.RequireDigit.Should().BeTrue();
        identity.Password.RequiredLength.Should().Be(8);
        identity.Lockout.MaxFailedAccessAttempts.Should().Be(5);
        identity.User.RequireUniqueEmail.Should().BeTrue();
    }
}
