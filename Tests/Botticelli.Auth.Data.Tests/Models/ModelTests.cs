using Botticelli.Auth.Data.Models;
using FluentAssertions;
using NUnit.Framework;

namespace Botticelli.Auth.Data.Tests.Models;

[TestFixture]
public class ModelTests
{
    [Test]
    public void BotUserRole_Should_ExposeAssignableProperties()
    {
        var id = Guid.Parse("d9887829-61a7-4947-9eb6-7faa66363f08");
        var role = new BotUserRole
        {
            Id = id,
            RoleName = "Admin",
            Description = "Bot users administrator",
            IsSuperUser = true
        };

        role.Id.Should().Be(id);
        role.RoleName.Should().Be("Admin");
        role.Description.Should().Be("Bot users administrator");
        role.IsSuperUser.Should().BeTrue();
    }

    [Test]
    public void BotUserRole_Should_DefaultIsSuperUserToFalse()
    {
        var role = new BotUserRole
        {
            Id = Guid.NewGuid(),
            RoleName = "Guest",
            Description = "guest"
        };

        role.IsSuperUser.Should().BeFalse();
    }

    [Test]
    public void BotUser_Should_ExposeAssignableProperties()
    {
        var roleId = Guid.NewGuid();
        var role = new BotUserRole
        {
            Id = roleId,
            RoleName = "User",
            Description = "user"
        };

        var user = new BotUser
        {
            UserId = "u-1",
            UserName = "alice",
            NickName = "ali",
            Email = "alice@example.com",
            FirstName = "Alice",
            LastName = "Smith",
            Phone = "+1000000",
            RoleId = roleId,
            Role = role,
            IsActive = true
        };

        user.UserId.Should().Be("u-1");
        user.UserName.Should().Be("alice");
        user.NickName.Should().Be("ali");
        user.Email.Should().Be("alice@example.com");
        user.FirstName.Should().Be("Alice");
        user.LastName.Should().Be("Smith");
        user.Phone.Should().Be("+1000000");
        user.RoleId.Should().Be(roleId);
        user.Role.Should().BeSameAs(role);
        user.IsActive.Should().BeTrue();
    }

    [Test]
    public void BotUser_Should_AllowNullOptionalFields()
    {
        var user = new BotUser
        {
            UserId = "u-2",
            UserName = "bob",
            RoleId = Guid.NewGuid(),
            IsActive = false,
            NickName = null,
            Email = null,
            FirstName = null,
            LastName = null,
            Phone = null,
            Role = null
        };

        user.NickName.Should().BeNull();
        user.Email.Should().BeNull();
        user.FirstName.Should().BeNull();
        user.LastName.Should().BeNull();
        user.Phone.Should().BeNull();
        user.Role.Should().BeNull();
        user.IsActive.Should().BeFalse();
    }

    [Test]
    public void AccessHistory_Should_ExposeAssignableProperties()
    {
        var timestamp = new DateTime(2026, 11, 20, 12, 0, 0, DateTimeKind.Utc);
        var entity = new BotUser
        {
            UserId = "u-3",
            UserName = "carol",
            RoleId = Guid.NewGuid(),
            IsActive = true
        };

        var history = new AccessHistory<BotUser>
        {
            Id = Guid.NewGuid(),
            Entity = entity,
            TimestampUtc = timestamp,
            IsSuccess = false,
            ErrorMessage = "denied"
        };

        history.Entity.Should().BeSameAs(entity);
        history.TimestampUtc.Should().Be(timestamp);
        history.IsSuccess.Should().BeFalse();
        history.ErrorMessage.Should().Be("denied");
        history.Id.Should().NotBeEmpty();
    }

    [Test]
    public void AccessHistory_Should_AllowNullOptionalFields()
    {
        var history = new AccessHistory<BotUser>
        {
            TimestampUtc = DateTime.UtcNow,
            Entity = null,
            ErrorMessage = null,
            IsSuccess = true
        };

        history.Entity.Should().BeNull();
        history.ErrorMessage.Should().BeNull();
        history.IsSuccess.Should().BeTrue();
    }
}
