using Botticelli.Chained.Context.InMemory;
using FluentAssertions;

namespace Botticelli.Chained.Context.InMemory.Tests;

[TestFixture]
public class InMemoryContextStorageBuilderTests
{
    [Test]
    public void Build_ShouldCreateStorage_WithDefaultCapacity()
    {
        var builder = new InMemoryContextStorageBuilder<string, int>();

        var storage = builder.Build();

        storage.Should().NotBeNull();
        storage.ContainsKey("any").Should().BeFalse();
    }

    [Test]
    public void Build_ShouldRespectInitialCapacity()
    {
        var builder = new InMemoryContextStorageBuilder<string, int>()
            .SetInitialCapacity(100);

        var storage = builder.Build();

        storage.Should().NotBeNull();
    }
}

