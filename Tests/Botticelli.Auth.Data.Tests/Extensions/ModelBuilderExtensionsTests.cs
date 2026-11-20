using Botticelli.Auth.Data.Extensions;
using Botticelli.Auth.Data.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Botticelli.Auth.Data.Tests.Extensions;

[TestFixture]
public class ModelBuilderExtensionsTests
{
    [Test]
    public void AddAuthenticationModels_Should_ConfigureKeysIndexesAndRelationships()
    {
        var options = new DbContextOptionsBuilder<TestAuthModelContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new TestAuthModelContext(options);
        var model = context.Model;

        var user = model.FindEntityType(typeof(BotUser));
        var role = model.FindEntityType(typeof(BotUserRole));

        user.Should().NotBeNull();
        role.Should().NotBeNull();

        user!.FindPrimaryKey()!.Properties.Select(p => p.Name).Should().Equal("UserId");
        role!.GetIndexes().Should().ContainSingle(i => i.IsUnique && i.Properties.Any(p => p.Name == "RoleName"));

        var roleNav = user.FindNavigation(nameof(BotUser.Role));
        roleNav.Should().NotBeNull();
        roleNav!.ForeignKey.PrincipalKey.Properties.Select(p => p.Name).Should().Equal("Id");
    }

    private sealed class TestAuthModelContext(DbContextOptions<TestAuthModelContext> options) : DbContext(options)
    {
        public DbSet<BotUser> BotUsers => Set<BotUser>();
        public DbSet<BotUserRole> BotUserRoles => Set<BotUserRole>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AddAuthenticationModels().Should().BeSameAs(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }
    }
}
