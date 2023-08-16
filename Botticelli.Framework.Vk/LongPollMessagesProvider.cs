using Botticelli.BotBase.Exceptions;
using Botticelli.Framework.Vk.API.Responses;
using Botticelli.Framework.Vk.Options;
using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using System.Net.Http.Json;
using Botticelli.Framework.Vk.API.Utils;

namespace Botticelli.Framework.Vk
{

    /// <summary>
    /// Long poll method provider
    /// https://dev.vk.com/ru/api/bots/getting-started
    /// 
    /// </summary>
    public class LongPollMessagesProvider : IDisposable
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<LongPollMessagesProvider> _logger;
        private readonly IOptionsMonitor<VkBotSettings> _settings;
        private HttpClient _client;
        private string _key;
        private string _server;
        private string _apiKey;
        private CancellationTokenSource _tokenSource;

        public LongPollMessagesProvider(IOptionsMonitor<VkBotSettings> settings, 
                                IHttpClientFactory httpClientFactory, 
                                ILogger<LongPollMessagesProvider> logger)
        {
            _settings = settings;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _tokenSource = new CancellationTokenSource();
        }

        public delegate void GotUpdates(UpdatesEventArgs  args);

        public event GotUpdates OnUpdates;

        public void SetApiKey(string key)
            => _apiKey  = key;

        public async Task Start()
        {
            try
            {
                _client = _httpClientFactory.CreateClient();
              
                // 1. Get Session
                await GetSessionData();
                
                // 2. Start polling
                if (string.IsNullOrWhiteSpace(_key) || string.IsNullOrWhiteSpace(_server)) 
                    throw new BotException($"{nameof(_key)} or {nameof(_server)} are null or empty!");
                int[] httpStatusCodesWorthRetrying = { 408, 500, 502, 503, 504 };

                var updatePolicy = Policy
                                      .Handle<FlurlHttpException>(ex =>
                                      {
                                          _logger.LogError(ex, $"Long polling error! session: {_key}, server: {_server}");

                                          return httpStatusCodesWorthRetrying.Contains(ex.Call.Response.StatusCode);
                                      })
                                      .WaitAndRetryAsync(3, n => n * TimeSpan.FromMilliseconds(_settings.CurrentValue.PollIntervalMs));


                var repeatPolicy = Policy.HandleResult<UpdatesResponse>(r => r.Updates != default)
                                         .WaitAndRetryForeverAsync(_ => TimeSpan.FromMilliseconds(_settings.CurrentValue.PollIntervalMs));


                var pollingTask = repeatPolicy.WrapAsync(updatePolicy)
                                                 .ExecuteAsync(async () =>
                                                 {
                                                     var url = _server.SetQueryParams(new
                                                     {
                                                         act = "a_check",
                                                         key = _key,
                                                         wait = 25,
                                                         mode = 2 + 8 + 32 + 64 + 128,
                                                         v = ApiVersion
                                                     });
                                                     var updates = await $"https://{_server}".SetQueryParams(new
                                                                   {
                                                                       act = "a_check",
                                                                       key = _key,
                                                                       wait = 25,
                                                                       mode = 2 + 8 + 32 + 64 + 128,
                                                                       v = ApiVersion
                                                                   })
                                                                   .GetJsonAsync<UpdatesResponse>(_tokenSource.Token);

                                                     if (updates.Updates != default)
                                                        OnUpdates?.Invoke(new UpdatesEventArgs(updates));

                                                     return updates;
                                                 });

                await pollingTask;
            }
            catch (Exception ex) when (ex is not BotException)
            {
                _logger.LogError(ex, $"Can't start a {nameof(LongPollMessagesProvider)}!");
            }

        }

        private async Task GetSessionData()
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
                                                 ApiUtils.GetMethodUri("https://api.vk.com",
                                                              "messages.getLongPollServer",
                                                              new
                                                              {
                                                                  access_token = _apiKey,
                                                                  v = ApiVersion
                                                              }));
            var response = await _client.SendAsync(request, _tokenSource.Token);
            var result = await response.Content.ReadFromJsonAsync<GetMessageSessionDataResponse>();
            // var rr = response.Content.ReadAsStringAsync();
            _server = result?.Response?.Server;
            _key = result?.Response?.Key;
        }

        public async Task Stop()
        {
            _client?.CancelPendingRequests();
            _tokenSource.Cancel(false);
            _key = string.Empty;
            _server = string.Empty;
        }

        public void Dispose()
        {
            var stopTask = Stop();
            stopTask.Wait(5000);
            _client?.Dispose();
        }

        private string ApiVersion => "5.81";
    }
}