using Botticelli.Server.Data.Entities.Auth;
using FluentAssertions;
using NUnit.Framework;

namespace Botticelli.Server.Data.Entities.Tests;

[TestFixture]
public class AuthEntitiesTests
{
    [Test]
    public void ApplicationRole_Should_HoldValues()
    {
        var id = Guid.NewGuid();
        var role = new ApplicationRole { Id = id, Name = "admin" };

        role.Id.Should().Be(id);
        role.Name.Should().Be("admin");
    }

    [Test]
    public void ApplicationUserGet_Should_HoldValues()
    {
        var id = Guid.NewGuid();
        var roleId = Guid.NewGuid();
        var user = new ApplicationUserGet
        {
            Id = id,
            FullName = "John",
            Email = "a@b.c",
            UserRoleIds = [roleId]
        };

        user.Id.Should().Be(id);
        user.FullName.Should().Be("John");
        user.Email.Should().Be("a@b.c");
        user.UserRoleIds.Should().ContainSingle().Which.Should().Be(roleId);
    }

    [Test]
    public void ApplicationUserPost_Should_HoldValues()
    {
        var post = new ApplicationUserPost
        {
            UserName = "u",
            StatusId = 2,
            UserRoleIds = ["r1"]
        };

        post.UserName.Should().Be("u");
        post.StatusId.Should().Be(2);
        post.UserRoleIds.Should().ContainSingle("r1");
    }

    [Test]
    public void ApplicationUserRoleGet_Should_HoldUserAndRole()
    {
        var link = new ApplicationUserRoleGet
        {
            User = new ApplicationUserGet { Id = Guid.NewGuid(), Email = "e" },
            Role = new ApplicationRole { Id = Guid.NewGuid(), Name = "viewer" }
        };

        link.User!.Email.Should().Be("e");
        link.Role!.Name.Should().Be("viewer");
    }

    [Test]
    public void ConfirmEmailRequest_Should_HoldValues()
    {
        var req = new ConfirmEmailRequest { Email = "e@x.com", Token = "tok" };
        req.Email.Should().Be("e@x.com");
        req.Token.Should().Be("tok");
    }

    [Test]
    public void DefaultUserAddRequest_And_Response_Should_HoldValues()
    {
        var req = new DefaultUserAddRequest { UserName = "u", Email = "e" };
        var resp = new DefaultUserAddResponse { Password = "p" };

        req.UserName.Should().Be("u");
        req.Email.Should().Be("e");
        resp.Password.Should().Be("p");
    }

    [Test]
    public void PasswordDeliveryMode_Should_HaveExpectedValues()
    {
        PasswordDeliveryMode.Email.Should().BeDefined();
        PasswordDeliveryMode.RegistrationWindow.Should().BeDefined();
        Enum.GetValues<PasswordDeliveryMode>().Should().HaveCount(2);
    }

    [Test]
    public void RegeneratePasswordRequest_Should_HoldValues()
    {
        var req = new RegeneratePasswordRequest { Email = "e", UserName = "u" };
        req.Email.Should().Be("e");
        req.UserName.Should().Be("u");
    }

    [Test]
    public void RegistrationSettingsResponse_Should_HoldMode()
    {
        var resp = new RegistrationSettingsResponse
        {
            PasswordDeliveryMode = PasswordDeliveryMode.RegistrationWindow
        };

        resp.PasswordDeliveryMode.Should().Be(PasswordDeliveryMode.RegistrationWindow);
    }

    [Test]
    public void UserAddRequest_Should_InheritAndHoldPassword()
    {
        var req = new UserAddRequest
        {
            UserName = "u",
            Email = "e",
            Password = "secret"
        };

        req.UserName.Should().Be("u");
        req.Email.Should().Be("e");
        req.Password.Should().Be("secret");
    }

    [Test]
    public void UserDeleteAndGetRequests_Should_HoldUserName()
    {
        new UserDeleteRequest { UserName = "del" }.UserName.Should().Be("del");
        new UserGetRequest { UserName = "get" }.UserName.Should().Be("get");
    }

    [Test]
    public void UserGetResponse_Should_HoldValues()
    {
        var resp = new UserGetResponse { UserName = "u", Email = "e" };
        resp.UserName.Should().Be("u");
        resp.Email.Should().Be("e");
    }

    [Test]
    public void UserLoginRequest_Should_HoldCredentials()
    {
        var req = new UserLoginRequest { Email = "e", Password = "p" };
        req.Email.Should().Be("e");
        req.Password.Should().Be("p");
    }

    [Test]
    public void UserUpdateRequest_Should_HoldValues()
    {
        var req = new UserUpdateRequest
        {
            UserName = "u",
            Email = "e",
            Password = "p"
        };

        req.UserName.Should().Be("u");
        req.Email.Should().Be("e");
        req.Password.Should().Be("p");
    }
}
