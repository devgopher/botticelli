using Botticelli.Talks.Exceptions;
using FluentAssertions;
using NUnit.Framework;

namespace Botticelli.Talks.Tests.Exceptions;

[TestFixture]
public class BotticelliTalksExceptionTests
{
    [Test]
    public void Constructor_Should_SetMessageAndInnerException()
    {
        var inner = new InvalidOperationException("inner");
        var ex = new BotticelliTalksException("failed", inner);

        ex.Message.Should().Be("failed");
        ex.InnerException.Should().BeSameAs(inner);
    }

    [Test]
    public void Constructor_Should_AllowNullInnerException()
    {
        var ex = new BotticelliTalksException("only-message");

        ex.Message.Should().Be("only-message");
        ex.InnerException.Should().BeNull();
    }
}
