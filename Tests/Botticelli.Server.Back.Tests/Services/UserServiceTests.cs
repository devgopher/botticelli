using Botticelli.Server.Back.Services.Auth;
using Botticelli.Server.Back.Tests.Helpers;
using Botticelli.Server.Data.Entities.Auth;
using Botticelli.Server.Data.Exceptions;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NUnit.Framework;

namespace Botticelli.Server.Back.Tests.Services;

[TestFixture]
public class UserServiceTests
{
    private const string Salt = "salt";

    private static IConfiguration CreateConfig() =>
        new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Authorization:Salt"] = Salt
            })
            .Build();

    private static UserService CreateService(Data.ServerDataContext context, Mock<IConfirmationService>? confirmation = null)
    {
        confirmation ??= new Mock<IConfirmationService>();
        return new UserService(CreateConfig(),
                               context,
                               NullLogger<UserService>.Instance,
                               confirmation.Object);
    }

    [Test]
    public async Task HasUsers_Should_ReflectPresence()
    {
        await using var context = TestServerDataContextFactory.Create();
        var service = CreateService(context);
        (await service.HasUsers(CancellationToken.None)).Should().BeFalse();

        await service.AddAsync(new UserAddRequest
        {
            UserName = "admin",
            Email = "a@b.c",
            Password = "Passw0rd!"
        }, false, CancellationToken.None);

        (await service.HasUsers(CancellationToken.None)).Should().BeTrue();
    }

    [Test]
    public async Task CheckAndAddAsync_Should_AddFirstUserAndConfirmEmail()
    {
        await using var context = TestServerDataContextFactory.Create();
        var service = CreateService(context);

        var added = await service.CheckAndAddAsync(new UserAddRequest
        {
            UserName = "admin",
            Email = "a@b.c",
            Password = "Passw0rd!"
        }, CancellationToken.None);

        added.Should().BeTrue();
        context.ApplicationUsers.Single().EmailConfirmed.Should().BeTrue();
    }

    [Test]
    public async Task CheckAndAddAsync_Should_ReturnFalse_WhenUsersExist()
    {
        await using var context = TestServerDataContextFactory.Create();
        var service = CreateService(context);
        await service.AddAsync(new UserAddRequest
        {
            UserName = "admin",
            Email = "a@b.c",
            Password = "Passw0rd!"
        }, false, CancellationToken.None);

        var added = await service.CheckAndAddAsync(new UserAddRequest
        {
            UserName = "other",
            Email = "o@b.c",
            Password = "Passw0rd!"
        }, CancellationToken.None);

        added.Should().BeFalse();
    }

    [Test]
    public async Task AddAsync_Should_SendConfirmation_WhenRequested()
    {
        await using var context = TestServerDataContextFactory.Create();
        var confirmation = new Mock<IConfirmationService>();
        var service = CreateService(context, confirmation);

        await service.AddAsync(new UserAddRequest
        {
            UserName = "admin",
            Email = "a@b.c",
            Password = "Passw0rd!"
        }, true, CancellationToken.None);

        confirmation.Verify(c => c.SendConfirmationCode(It.IsAny<IdentityUser>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task AddAsync_Should_Throw_OnDuplicate()
    {
        await using var context = TestServerDataContextFactory.Create();
        var service = CreateService(context);
        var request = new UserAddRequest
        {
            UserName = "admin",
            Email = "a@b.c",
            Password = "Passw0rd!"
        };
        await service.AddAsync(request, false, CancellationToken.None);

        var act = async () => await service.AddAsync(request, false, CancellationToken.None);
        await act.Should().ThrowAsync<DataException>();
    }

    [Test]
    public async Task UpdateAsync_Should_ChangeEmailAndPassword()
    {
        await using var context = TestServerDataContextFactory.Create();
        var confirmation = new Mock<IConfirmationService>();
        var service = CreateService(context, confirmation);
        await service.AddAsync(new UserAddRequest
        {
            UserName = "admin",
            Email = "old@b.c",
            Password = "Passw0rd!"
        }, false, CancellationToken.None);

        await service.UpdateAsync(new UserUpdateRequest
        {
            UserName = "admin",
            Email = "new@b.c",
            Password = "NewPass1!"
        }, CancellationToken.None);

        context.ApplicationUsers.Single().Email.Should().Be("new@b.c");
        confirmation.Verify(c => c.SendConfirmationCode(It.IsAny<IdentityUser>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task DeleteAsync_Should_RemoveUser()
    {
        await using var context = TestServerDataContextFactory.Create();
        var service = CreateService(context);
        await service.AddAsync(new UserAddRequest
        {
            UserName = "admin",
            Email = "a@b.c",
            Password = "Passw0rd!"
        }, false, CancellationToken.None);

        await service.DeleteAsync(new UserDeleteRequest { UserName = "admin" }, CancellationToken.None);

        context.ApplicationUsers.Should().BeEmpty();
    }

    [Test]
    public async Task GetAsync_Should_ReturnUser()
    {
        await using var context = TestServerDataContextFactory.Create();
        var service = CreateService(context);
        await service.AddAsync(new UserAddRequest
        {
            UserName = "admin",
            Email = "a@b.c",
            Password = "Passw0rd!"
        }, false, CancellationToken.None);

        var user = await service.GetAsync(new UserGetRequest { UserName = "admin" }, CancellationToken.None);

        user.UserName.Should().Be("admin");
        user.Email.Should().Be("a@b.c");
    }

    [Test]
    public async Task ConfirmCodeAsync_Should_DelegateToConfirmationService()
    {
        await using var context = TestServerDataContextFactory.Create();
        var confirmation = new Mock<IConfirmationService>();
        confirmation.Setup(c => c.ConfirmCodeAsync("tok", It.IsAny<IdentityUser>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(true);
        var service = CreateService(context, confirmation);
        await service.AddAsync(new UserAddRequest
        {
            UserName = "admin",
            Email = "a@b.c",
            Password = "Passw0rd!"
        }, false, CancellationToken.None);

        var ok = await service.ConfirmCodeAsync("a@b.c", "tok", CancellationToken.None);

        ok.Should().BeTrue();
    }

    [Test]
    public async Task GetAsync_Should_Throw_WhenMissing()
    {
        await using var context = TestServerDataContextFactory.Create();
        var service = CreateService(context);

        var act = async () => await service.GetAsync(new UserGetRequest { UserName = "missing" }, CancellationToken.None);
        await act.Should().ThrowAsync<DataException>();
    }
}
