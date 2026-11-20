using Botticelli.Server.FrontNew.Utils;
using FluentAssertions;
using NUnit.Framework;

namespace Botticelli.Server.FrontNew.Tests.Utils;

[TestFixture]
public class ConstantsTests
{
    [Test]
    public void MaxAllowedFileSize_Should_Be100Mb()
    {
        Constants.MaxAllowedFileSize.Should().Be(104857600);
    }
}
