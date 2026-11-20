using System.Globalization;
using Botticelli.Server.FrontNew.Utils;
using FluentAssertions;
using NUnit.Framework;

namespace Botticelli.Server.FrontNew.Tests.Utils;

[TestFixture]
public class StringUtilsTests
{
    [Test]
    public void ToDateTime_Should_ParseExact()
    {
        var result = "2026-03-15".ToDateTime("yyyy-MM-dd", CultureInfo.InvariantCulture);

        result.Should().Be(new DateTime(2026, 3, 15));
    }

    [Test]
    public void ToDateTime_Should_ReturnNull_ForEmptyOrInvalid()
    {
        "".ToDateTime("yyyy-MM-dd", CultureInfo.InvariantCulture).Should().BeNull();
        "   ".ToDateTime("yyyy-MM-dd", CultureInfo.InvariantCulture).Should().BeNull();
        "not-a-date".ToDateTime("yyyy-MM-dd", CultureInfo.InvariantCulture).Should().BeNull();
    }
}
