using Botticelli.Server.Back.Controllers;
using Botticelli.Server.Back.Services.Auth;
using Botticelli.Server.Data.Entities.Auth;
using Botticelli.Server.Models.Responses;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace Botticelli.Server.Back.Tests.Controllers;

[TestFixture]
public class AuthControllerTests
{
    [Test]
    public void GetToken_Should_ReturnOkWithTokenResponse()
    {
        var auth = new Mock<IAdminAuthService>();
        auth.Setup(a => a.GenerateToken(It.IsAny<UserLoginRequest>()))
            .Returns(new GetTokenResponse { IsSuccess = true, Token = "jwt" });
        var controller = new AuthController(auth.Object);

        var result = controller.GetToken(new UserLoginRequest
        {
            Email = "a@b.c",
            Password = "p"
        });

        result.Should().BeOfType<OkObjectResult>()
              .Which.Value.Should().BeOfType<GetTokenResponse>()
              .Which.Token.Should().Be("jwt");
    }
}
