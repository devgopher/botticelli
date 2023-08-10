using Botticelli.Framework.Vk.API.Responses;
using Botticelli.Framework.Vk.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Botticelli.Framework.Vk
{

    /// <summary>
    /// Long poll method provider
    /// https://dev.vk.com/ru/api/bots/getting-started
    /// 
    /// </summary>
    public class LongPollProvider : IDisposable
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<LongPollProvider> _logger;
        private readonly IOptionsMonitor<VkBotSettings> _settings;
        private string _sessionId;

        public LongPollProvider(IOptionsMonitor<VkBotSettings> settings, 
                                IHttpClientFactory httpClientFactory, 
                                ILogger<LongPollProvider> logger)
        {
            _settings = settings;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public delegate void GotUpdates(UpdatesEventArgs  args);

        public event GotUpdates OnUpdates;

        public Task Start()
        {
            using var client = _httpClientFactory.CreateClient();

            client.BaseAddress =  new Uri("https://api.vk.com");

            // 1. Get Session
            


            // 2. Start polling

        }

        public void Dispose()
        {
        }
    }
}