using Botticelli.Talks.Constants;
using FluentAssertions;
using NUnit.Framework;

namespace Botticelli.Talks.Tests.Constants;

[TestFixture]
public class CompressionLevelsTests
{
    [Test]
    public void CompressionLevels_Should_HaveExpectedValues()
    {
        ((int)CompressionLevels.None).Should().Be(0);
        ((int)CompressionLevels.Low).Should().Be(1);
        ((int)CompressionLevels.Medium).Should().Be(2);
        ((int)CompressionLevels.High).Should().Be(3);
    }
}
