using Botticelli.Framework.Vk.Messages;
using Botticelli.Framework.Vk.Messages.Options;
using Botticelli.Framework.Vk.Tests.Settings;
using Botticelli.SecureStorage.Settings;
using Microsoft.Extensions.Configuration;
using Shared;
using NUnit.Framework;

namespace Botticelli.Framework.Vk.Tests;

[TestFixture]
public class LongPollMessagesProviderTests
{
    [SetUp]
    public void Setup()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var settings = config.GetSection(nameof(SampleSettings))
            .Get<SampleSettings>();

        _provider = new LongPollMessagesProvider(new OptionsMonitorMock<VkBotSettings>(new VkBotSettings
                                                 {
                                                     SecureStorageConnectionString = settings.SecureStorageConnectionString,
                                                     Name = "test",
                                                     PollIntervalMs = 500,
                                                     GroupId = 221973506
                                                 }),
            new TestHttpClientFactory(),
            LoggerMocks.CreateConsoleLogger<LongPollMessagesProvider>());
    }

    private LongPollMessagesProvider _provider;

    [Test]
    public async Task StartTest()
    {
        await _provider.Stop();
        _provider.SetApiKey(EnvironmentDataProvider.GetApiKey());

        var task = Task.Run(() => _provider.Start(CancellationToken.None));

        Thread.Sleep(5000);

        Assert.That(task.Exception == null);
    }

    [Test]
    public void StopTest()
    {
        _ = _provider.Start(CancellationToken.None);

        Thread.Sleep(2000);

        Assert.DoesNotThrowAsync(_provider.Stop);
    }
}