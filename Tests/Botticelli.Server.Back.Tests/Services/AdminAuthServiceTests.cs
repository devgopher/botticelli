using System.Security.Claims;
using Botticelli.Auth.Shared.Settings;
using Botticelli.Server.Back.Services.Auth;
using Botticelli.Server.Back.Tests.Helpers;
using Botticelli.Server.Data.Entities.Auth;
using Botticelli.Shared.Utils;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;

namespace Botticelli.Server.Back.Tests.Services;

[TestFixture]
public class AdminAuthServiceTests
{
    private const string Salt = "salt";
    private const string Key = "0123456789abcdef0123456789abcdef";

    private static IConfiguration CreateConfig() =>
        new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Authorization:Salt"] = Salt,
                ["Authorization:Key"] = Key,
                ["Authorization:Issuer"] = "issuer",
                ["Authorization:Audience"] = "audience"
            })
            .Build();

    private static IOptionsSnapshot<AuthSettings> CreateSettings(int lifetime = 30)
    {
        var snapshot = new Mock<IOptionsSnapshot<AuthSettings>>();
        snapshot.Setup(s => s.Value).Returns(new AuthSettings { TokenLifetimeMin = lifetime });
        return snapshot.Object;
    }

    private static AdminAuthService CreateService(
        Data.ServerDataContext context,
        IHttpContextAccessor? accessor = null) =>
        new(CreateConfig(),
            accessor ?? Mock.Of<IHttpContextAccessor>(),
            context,
            CreateSettings(),
            NullLogger<AdminAuthService>.Instance);

    [Test]
    public async Task HasUsersAsync_Should_ReflectUsers()
    {
        await using var context = TestServerDataContextFactory.Create();
        var service = CreateService(context);
        (await service.HasUsersAsync()).Should().BeFalse();

        context.ApplicationUsers.Add(new IdentityUser { Id = "1", Email = "a@b.c", UserName = "a" });
        await context.SaveChangesAsync();

        (await service.HasUsersAsync()).Should().BeTrue();
    }

    [Test]
    public async Task RegisterAsync_Should_CreateUserAndRole()
    {
        await using var context = TestServerDataContextFactory.Create();
        var service = CreateService(context);

        await service.RegisterAsync(new UserAddRequest
        {
            UserName = "admin",
            Email = "admin@test.com",
            Password = "Passw0rd!"
        });

        context.ApplicationUsers.Should().ContainSingle(u => u.Email == "admin@test.com");
        context.ApplicationUserRoles.Should().HaveCount(1);
    }

    [Test]
    public async Task RegisterAsync_Should_SwallowDuplicateEmail()
    {
        await using var context = TestServerDataContextFactory.Create();
        context.ApplicationUsers.Add(new IdentityUser
        {
            Id = "1",
            Email = "admin@test.com",
            NormalizedEmail = "ADMIN@TEST.COM",
            UserName = "admin"
        });
        await context.SaveChangesAsync();
        var service = CreateService(context);

        await service.RegisterAsync(new UserAddRequest
        {
            UserName = "admin2",
            Email = "admin@test.com",
            Password = "Passw0rd!"
        });

        context.ApplicationUsers.Should().ContainSingle();
    }

    [Test]
    public void GenerateToken_Should_ReturnToken_ForValidUser()
    {
        using var context = TestServerDataContextFactory.Create();
        var password = "Passw0rd!";
        var user = new IdentityUser
        {
            Id = Guid.NewGuid().ToString(),
            Email = "admin@test.com",
            NormalizedEmail = "ADMIN@TEST.COM",
            UserName = "admin@test.com",
            PasswordHash = HashUtils.GetHash(password, Salt),
            EmailConfirmed = true
        };
        context.ApplicationUsers.Add(user);
        var role = context.ApplicationRoles.First();
        context.ApplicationUserRoles.Add(new IdentityUserRole<string> { UserId = user.Id, RoleId = role.Id });
        context.SaveChanges();

        var service = CreateService(context);
        var token = service.GenerateToken(new UserLoginRequest
        {
            Email = "admin@test.com",
            Password = password
        });

        token.Should().NotBeNull();
        token!.IsSuccess.Should().BeTrue();
        token.Token.Should().NotBeNullOrWhiteSpace();
    }

    [Test]
    public void GenerateToken_Should_DenyInvalidPassword()
    {
        using var context = TestServerDataContextFactory.Create();
        context.ApplicationUsers.Add(new IdentityUser
        {
            Id = "1",
            Email = "admin@test.com",
            NormalizedEmail = "ADMIN@TEST.COM",
            UserName = "admin",
            PasswordHash = HashUtils.GetHash("good", Salt)
        });
        context.SaveChanges();
        var service = CreateService(context);

        var token = service.GenerateToken(new UserLoginRequest
        {
            Email = "admin@test.com",
            Password = "bad"
        });

        token.Should().NotBeNull();
        token!.IsSuccess.Should().BeFalse();
    }

    [Test]
    public void CheckToken_Should_ValidateGeneratedToken()
    {
        using var context = TestServerDataContextFactory.Create();
        var password = "Passw0rd!";
        context.ApplicationUsers.Add(new IdentityUser
        {
            Id = Guid.NewGuid().ToString(),
            Email = "admin@test.com",
            NormalizedEmail = "ADMIN@TEST.COM",
            UserName = "admin@test.com",
            PasswordHash = HashUtils.GetHash(password, Salt)
        });
        context.SaveChanges();
        var service = CreateService(context);
        var generated = service.GenerateToken(new UserLoginRequest
        {
            Email = "admin@test.com",
            Password = password
        })!;

        service.CheckToken(generated.Token!).Should().BeTrue();
        service.CheckToken("not-a-token").Should().BeFalse();
    }

    [Test]
    public void CheckAccess_Should_RespectEmailConfirmation()
    {
        using var context = TestServerDataContextFactory.Create();
        var password = "Passw0rd!";
        context.ApplicationUsers.Add(new IdentityUser
        {
            Id = "1",
            Email = "admin@test.com",
            NormalizedEmail = "ADMIN@TEST.COM",
            UserName = "admin",
            PasswordHash = HashUtils.GetHash(password, Salt),
            EmailConfirmed = false
        });
        context.SaveChanges();
        var service = CreateService(context);
        var login = new UserLoginRequest { Email = "admin@test.com", Password = password };

        service.CheckAccess(login, false).result.Should().BeTrue();
        var denied = service.CheckAccess(login, true);
        denied.result.Should().BeFalse();
        denied.err.Should().Contain("not confirmed");
    }

    [Test]
    public void GetCurrentUserId_Should_ReadClaim()
    {
        using var context = TestServerDataContextFactory.Create();
        var accessor = new Mock<IHttpContextAccessor>();
        accessor.Setup(a => a.HttpContext).Returns(new DefaultHttpContext
        {
            User = new ClaimsPrincipal(new ClaimsIdentity([
                new Claim("applicationUserId", "user-42")
            ]))
        });
        var service = CreateService(context, accessor.Object);

        service.GetCurrentUserId().Should().Be("user-42");
    }

    [Test]
    public async Task RegeneratePassword_Should_CompleteWithoutThrowing()
    {
        await using var context = TestServerDataContextFactory.Create();
        var service = CreateService(context);

        await service.RegeneratePassword(new UserAddRequest
        {
            UserName = "u",
            Email = "new@test.com",
            Password = "Passw0rd!"
        });
    }
}
