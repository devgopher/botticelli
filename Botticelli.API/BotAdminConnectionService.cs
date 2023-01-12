using Botticelli.BotBase.Exceptions;
using Botticelli.BotBase.Settings;
using Botticelli.Shared.API.Admin.Requests;
using Botticelli.Shared.API.Admin.Responses;
using Newtonsoft.Json;

namespace Botticelli.BotBase
{
    /// <summary>
    /// This service is intended for sending keepalive/hello messages
    /// to Botticelli Admin server
    /// </summary>
    internal class BotAdminConnectionService : IHostedService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ServerSettings _serverSettings;
        private Task _keepAliveTask; 
        private readonly ManualResetEventSlim _keepAliveEvent = new(false);

        public BotAdminConnectionService(IHttpClientFactory httpClientFactory,
            ServerSettings serverSettings)
        {
            _httpClientFactory = httpClientFactory;
            _serverSettings = serverSettings;
            _keepAliveEvent.Reset();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            if (_keepAliveTask == default)
            {
                _keepAliveEvent.Set();
                _keepAliveTask = Task.Run(async () =>
                {
                    int unsuccessCounter = 0;

                    while (!cancellationToken.IsCancellationRequested || _keepAliveEvent.IsSet)
                    {
                        var request = new KeepAliveNotificationRequest
                        {
                            BotId = Utils.BotDataUtils.GetBotId()
                        };

                        var response = await InnerSend<KeepAliveNotificationRequest, KeepAliveNotificationResponse>(request, "KeepAlive", cancellationToken);

                        if (!response.IsSuccess)
                        {
                            ++unsuccessCounter;
                            if (unsuccessCounter == 5)
                            {
                                // TODO: log

                                break;
                            }
                        }

                        Thread.Sleep(15000);
                    }
                }, cancellationToken);


            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _keepAliveEvent.Reset();
        }

        private async Task<TResp> InnerSend<TReq, TResp>(TReq request,
            string funcName,
            CancellationToken cancellationToken)
        {
            using var httpClient = _httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(_serverSettings.ServerUri);

            var content = JsonContent.Create(request);

            var httpResponse = await httpClient.PostAsync(funcName, content, cancellationToken);

            if (!httpResponse.IsSuccessStatusCode)
                throw new BotException(
                    $"Error sending request to a Botticelli server!");

            var responseJson = await httpResponse.Content.ReadAsStringAsync(cancellationToken);

            return JsonConvert.DeserializeObject<TResp>(responseJson);
        }
    }
}
