using Botticelli.Server.FrontNew.Utils;
using FluentAssertions;
using NUnit.Framework;

namespace Botticelli.Server.FrontNew.Tests.Utils;

[TestFixture]
public class ProgressStreamContentTests
{
    [Test]
    public async Task SerializeToStreamAsync_Should_ReportProgress()
    {
        var payload = new byte[20_000];
        Random.Shared.NextBytes(payload);
        await using var source = new MemoryStream(payload);
        var reports = new List<(long read, long total)>();
        using var content = new ProgressStreamContent(source, (read, total) => reports.Add((read, total)));
        content.Headers.ContentLength = payload.Length;

        await using var destination = new MemoryStream();
        await content.CopyToAsync(destination);

        destination.ToArray().Should().Equal(payload);
        reports.Should().NotBeEmpty();
        reports.Last().read.Should().Be(payload.Length);
    }
}
