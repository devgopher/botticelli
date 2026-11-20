using Botticelli.Server.Data.Exceptions;
using FluentAssertions;
using NUnit.Framework;

namespace Botticelli.Server.Data.Tests;

[TestFixture]
public class DataExceptionTests
{
    [Test]
    public void DataException_Should_PreserveMessage()
    {
        var ex = new DataException("boom");

        ex.Message.Should().Be("boom");
        ex.InnerException.Should().BeNull();
    }

    [Test]
    public void DataException_Should_PreserveInnerException()
    {
        var inner = new InvalidOperationException("inner");
        var ex = new DataException("outer", inner);

        ex.Message.Should().Be("outer");
        ex.InnerException.Should().BeSameAs(inner);
    }
}
