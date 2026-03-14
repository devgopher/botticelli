using System;
using Botticelli.Chained.Context;
using Botticelli.Chained.Context.InMemory;
using FluentAssertions;

namespace Botticelli.Chained.Context.InMemory.Tests;

[TestFixture]
public class CommandContextTests
{
    private CommandContext _context = null!;

    [SetUp]
    public void SetUp()
    {
        var storage = new InMemoryStorage<string, string>();
        _context = new CommandContext(storage);
    }

    [Test]
    public void Set_ShouldThrow_WhenValueIsNull()
    {
        Action act = () => _context.Set<string>("key", null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void SetAndGet_ShouldRoundtripNonArgsValue_UsingJson()
    {
        var value = new Sample { Name = "test", Number = 42 };

        _context.Set("sample", value);

        var result = _context.Get<Sample>("sample");

        result.Should().NotBeNull();
        result!.Name.Should().Be("test");
        result.Number.Should().Be(42);
    }

    [Test]
    public void SetAndGet_ShouldUsePlainString_ForArgsName()
    {
        _context.Set(Names.Args, "hello");
        
        _context.Get(Names.Args).Should().Be("hello");
    }

    [TestCase(1.23)]
    [TestCase(1234.5)]
    public void SetAndGet_ShouldHandleDouble_WithInvariantFormatting(double value)
    {
        _context.Set(Names.Args, value);

        var stored = _context.Get(Names.Args);
        stored.Should().Contain(".");

        var roundtripped = _context.Get<double>(Names.Args);
        roundtripped.Should().Be(value);
    }

    [Test]
    public void Get_ShouldReturnDefault_WhenKeyNotFound()
    {
        _context.Get<string>("missing").Should().BeNull();
        _context.Get<int>("missing").Should().Be(0);
    }

    [Test]
    public void Transform_ShouldThrow_WhenKeyNotFound()
    {
        Action act = () => _context.Transform<int>("missing", x => x + 1);

        act.Should().Throw<KeyNotFoundException>();
    }

    [Test]
    public void Transform_ShouldApplyFunction_AndUpdateStoredValue()
    {
        _context.Set("value", 10);

        var original = _context.Transform<int>("value", x => x + 5);

        original.Should().Be(10);
        _context.Get<int>("value").Should().Be(15);
    }

    private sealed class Sample
    {
        public string Name { get; set; } = string.Empty;
        public int Number { get; set; }
    }
}

