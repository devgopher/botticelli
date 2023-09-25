using Botticelli.Framework.Vk.Messages;
using Botticelli.Framework.Vk.Messages.API.Requests;
using NUnit.Framework;

namespace Botticelli.Framework.Vk.Tests;

[TestFixture]
public class MessagePublisherTests
{
    [SetUp]
    public void Setup()
    {
        _publisher = new MessagePublisher(new TestHttpClientFactory(),
                                          Utils.CreateConsoleLogger<MessagePublisher>());
    }

    private MessagePublisher _publisher;

    [Test]
    public async Task SendAsyncTest()
    {
        _publisher.SetApiKey(EnvironmentDataProvider.GetApiKey());
        Assert.DoesNotThrowAsync(async () => await _publisher.SendAsync(new VkSendMessageRequest
                                                                        {
                                                                            AccessToken = EnvironmentDataProvider.GetApiKey(),
                                                                            Body = $"test msg {DateTime.Now.ToString()}",
                                                                            UserId = EnvironmentDataProvider.GetTargetUserId().ToString()
                                                                        },
                                                                        CancellationToken.None));
    }
}