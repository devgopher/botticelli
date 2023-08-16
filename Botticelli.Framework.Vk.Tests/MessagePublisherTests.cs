using Botticelli.Framework.Vk.API.Requests;
using NUnit.Framework;

namespace Botticelli.Framework.Vk.Tests
{
    [TestFixture()]
    public class MessagePublisherTests
    {
        private MessagePublisher _publisher;

        [SetUp]
        public void Setup()
        {
            _publisher = new MessagePublisher(new TestHttpClientFactory(),
                                              Utils.CreateConsoleLogger<MessagePublisher>());
        }

        [Test()]
        public async Task SendAsyncTest()
        {
            _publisher.SetApiKey(EnvironmentDataProvider.GetApiKey());
            Assert.DoesNotThrowAsync( async () => await _publisher.SendAsync(new SendMessageRequest()
            {
                AccessToken = EnvironmentDataProvider.GetApiKey(),
                Body = $"test msg {DateTime.Now.ToString()}",
                UserId = EnvironmentDataProvider.GetTargetUserId().ToString()
            }, CancellationToken.None));
        }
    }
}