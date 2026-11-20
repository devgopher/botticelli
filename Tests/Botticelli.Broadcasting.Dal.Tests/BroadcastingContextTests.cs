using Botticelli.Broadcasting.Dal;
using Botticelli.Broadcasting.Dal.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Botticelli.Broadcasting.Dal.Tests;

[TestFixture]
public class BroadcastingContextTests
{
    private static BroadcastingContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<BroadcastingContext>()
            .UseSqlite($"Data Source={Path.Combine(Path.GetTempPath(), $"bcast-dal-{Guid.NewGuid():N}.db")}")
            .Options;

        var context = new BroadcastingContext(options);
        context.Database.EnsureCreated();
        return context;
    }

    [Test]
    public void ParameterlessConstructor_Should_CreateInstance()
    {
        var context = new BroadcastingContext();
        context.Should().NotBeNull();
    }

    [Test]
    public async Task Chats_Should_PersistAndQuery()
    {
        await using var context = CreateContext();

        context.Chats.Add(new Chat { ChatId = "c1", IsActive = true });
        context.Chats.Add(new Chat { ChatId = "c2", IsActive = false });
        await context.SaveChangesAsync();

        var active = await context.Chats.Where(c => c.IsActive).ToListAsync();

        active.Should().ContainSingle(c => c.ChatId == "c1");
    }

    [Test]
    public void OnModelCreating_Should_ConfigureMessageStatusCompositeKey()
    {
        using var context = CreateContext();

        var entity = context.Model.FindEntityType(typeof(MessageStatus));
        entity.Should().NotBeNull();

        var key = entity!.FindPrimaryKey();
        key.Should().NotBeNull();
        key!.Properties.Select(p => p.Name).Should().BeEquivalentTo("ChatId", "MessageId");
    }
}
