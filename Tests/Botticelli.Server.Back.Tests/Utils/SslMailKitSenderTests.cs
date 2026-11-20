using Botticelli.Server.Back.Utils;
using FluentAssertions;
using FluentEmail.Core;
using FluentEmail.MailKitSmtp;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;

namespace Botticelli.Server.Back.Tests.Utils;

[TestFixture]
public class SslMailKitSenderTests
{
    private string _pickupDir = null!;

    [SetUp]
    public void SetUp()
    {
        _pickupDir = Path.Combine(Path.GetTempPath(), "botticelli-mail-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_pickupDir);
    }

    [TearDown]
    public void TearDown()
    {
        if (Directory.Exists(_pickupDir))
            Directory.Delete(_pickupDir, true);
    }

    private SslMailKitSender CreateSender(bool usePickup = true)
    {
        var options = new Mock<IOptionsMonitor<SmtpClientOptions>>();
        options.Setup(o => o.CurrentValue).Returns(new SmtpClientOptions
        {
            UsePickupDirectory = usePickup,
            MailPickupDirectory = _pickupDir,
            Server = "localhost",
            Port = 25,
            UseSsl = false
        });
        return new SslMailKitSender(options.Object);
    }

    private static IFluentEmail CreateEmail() =>
        Email.From("from@test.com", "From")
             .To("to@test.com")
             .Subject("subj")
             .Body("hello body");

    [Test]
    public void Send_Should_WritePickupFile()
    {
        var sender = CreateSender();
        var response = sender.Send(CreateEmail());

        response.Successful.Should().BeTrue();
        Directory.GetFiles(_pickupDir, "*.eml").Should().NotBeEmpty();
    }

    [Test]
    public async Task SendAsync_Should_WritePickupFile()
    {
        var sender = CreateSender();
        var response = await sender.SendAsync(CreateEmail());

        response.Successful.Should().BeTrue();
        Directory.GetFiles(_pickupDir, "*.eml").Should().NotBeEmpty();
    }

    [Test]
    public void Send_Should_ReturnError_WhenCancelled()
    {
        var sender = CreateSender();
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        var response = sender.Send(CreateEmail(), cts.Token);

        response.Successful.Should().BeFalse();
        response.ErrorMessages.Should().Contain(m => m.Contains("cancelled", StringComparison.OrdinalIgnoreCase));
    }

    [Test]
    public async Task SendAsync_Should_ReturnError_WhenCancelled()
    {
        var sender = CreateSender();
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        var response = await sender.SendAsync(CreateEmail(), cts.Token);

        response.Successful.Should().BeFalse();
        response.ErrorMessages.Should().Contain(m => m.Contains("cancelled", StringComparison.OrdinalIgnoreCase));
    }

    [Test]
    public async Task SendAsync_Should_CaptureSmtpErrors_WhenNotPickup()
    {
        var sender = CreateSender(usePickup: false);
        var response = await sender.SendAsync(CreateEmail());

        response.Successful.Should().BeFalse();
        response.ErrorMessages.Should().NotBeEmpty();
    }

    [Test]
    public void Send_Should_SupportHtmlAndAttachments()
    {
        var sender = CreateSender();
        using var stream = new MemoryStream([1, 2, 3]);
        var email = Email.From("from@test.com")
                         .To("to@test.com")
                         .CC("cc@test.com")
                         .BCC("bcc@test.com")
                         .ReplyTo("reply@test.com")
                         .Subject("html")
                         .Body("<b>hi</b>", true)
                         .Attach(new FluentEmail.Core.Models.Attachment
                         {
                             Filename = "a.txt",
                             Data = stream,
                             ContentType = "text/plain",
                             ContentId = "cid1"
                         });

        var response = sender.Send(email);
        response.Successful.Should().BeTrue();
    }
}
