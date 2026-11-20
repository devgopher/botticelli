using Botticelli.Auth.Data;
using Botticelli.Auth.Data.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using NUnit.Framework;

namespace Botticelli.Auth.Data.Tests;

[TestFixture]
public class AuthDefaultDbContextTests
{
    [Test]
    public void OnModelCreating_Should_SeedDefaultRolesAndConfigureSchema()
    {
        var options = new DbContextOptionsBuilder<AuthDefaultDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new AuthDefaultDbContext(options);
        var model = context.GetService<IDesignTimeModel>().Model;

        model.GetDefaultSchema().Should().Be("Botticelli.Auth.Sample.Telegram");

        var roleEntity = model.FindEntityType(typeof(BotUserRole));
        roleEntity.Should().NotBeNull();

        var seed = roleEntity!.GetSeedData().ToList();
        seed.Should().HaveCount(3);
        seed.Select(d => d["RoleName"]?.ToString())
            .Should()
            .BeEquivalentTo(DefaultRoles.Admin, DefaultRoles.Guest, DefaultRoles.User);

        var adminId = Guid.Parse("d9887829-61a7-4947-9eb6-7faa66363f08");
        seed.Should().Contain(d =>
            Equals(d["Id"], adminId) &&
            Equals(d["IsSuperUser"], true));

        model.FindEntityType(typeof(BotUser)).Should().NotBeNull();
        model.FindEntityType(typeof(AccessHistory<BotUser>)).Should().NotBeNull();
    }

    [Test]
    public void AuthDefaultDbContext_Should_AcceptOptions()
    {
        var options = new DbContextOptionsBuilder<AuthDefaultDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new AuthDefaultDbContext(options);
        context.Should().NotBeNull();
        context.Database.IsInMemory().Should().BeTrue();
    }
}
