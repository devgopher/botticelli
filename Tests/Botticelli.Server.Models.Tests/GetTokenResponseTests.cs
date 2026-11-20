using Botticelli.Server.Models.Responses;
using FluentAssertions;
using NUnit.Framework;

namespace Botticelli.Server.Models.Tests;

[TestFixture]
public class GetTokenResponseTests
{
    [Test]
    public void GetTokenResponse_Should_ExposeAssignableProperties()
    {
        var response = new GetTokenResponse
        {
            IsSuccess = true,
            Token = "jwt-token"
        };

        response.IsSuccess.Should().BeTrue();
        response.Token.Should().Be("jwt-token");
    }

    [Test]
    public void GetTokenResponse_Should_AllowNullToken()
    {
        var response = new GetTokenResponse
        {
            IsSuccess = false,
            Token = null
        };

        response.IsSuccess.Should().BeFalse();
        response.Token.Should().BeNull();
    }
}
