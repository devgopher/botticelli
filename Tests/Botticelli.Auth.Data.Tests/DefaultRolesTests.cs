using Botticelli.Auth.Data;
using FluentAssertions;
using NUnit.Framework;

namespace Botticelli.Auth.Data.Tests;

[TestFixture]
public class DefaultRolesTests
{
    [Test]
    public void DefaultRoles_Should_ExposeExpectedConstants()
    {
        DefaultRoles.Guest.Should().Be("Guest");
        DefaultRoles.User.Should().Be("User");
        DefaultRoles.Admin.Should().Be("Admin");
    }
}
