using System.Net.Http.Json;
using Botticelli.BotBase.Exceptions;
using Botticelli.BotBase.Settings;
using Botticelli.Interfaces;
using Botticelli.Shared.API.Admin.Requests;
using Botticelli.Shared.API.Admin.Responses;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace Botticelli.BotBase
{
    /// <summary>
    /// This service is intended for sending keepalive/hello messages
    /// to Botticelli Admin server and receiving status messages from it
    /// </summary>
    internal class BotStatusService : IHostedService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IServiceProvider _serviceProvider;
        private readonly ServerSettings _serverSettings;
        private Task _keepAliveTask;
        private Task _getRequiredStatusEventTask;
        private readonly ManualResetEventSlim _keepAliveEvent = new(false);
        private readonly ManualResetEventSlim _getRequiredStatusEvent = new(false);
        private const short MaxRetryCount = 5;
        private const short PausePeriod = 5000;
        private readonly string? _botId;
        private IEnumerable<IBot> _subBots;

        public BotStatusService()
        {}

        public BotStatusService(IHttpClientFactory httpClientFactory,
            IServiceProvider serviceProvider,
            ServerSettings serverSettings)
        {
            _httpClientFactory = httpClientFactory;
            _serviceProvider = serviceProvider;
            _serverSettings = serverSettings;
            _keepAliveEvent.Reset();
            _getRequiredStatusEvent.Reset();
            _botId = Utils.BotDataUtils.GetBotId();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            KeepAlive(cancellationToken);
            GetRequiredStatus(cancellationToken);
        }

        /// <summary>
        /// Sends KeepAlive messages
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <exception cref="BotException"></exception>
        private void KeepAlive(CancellationToken cancellationToken)
        {
            if (_keepAliveTask != default)
                return;

            _keepAliveEvent.Set();
            _keepAliveTask = Task.Run(async () =>
            {
                var request = new KeepAliveNotificationRequest
                {
                    BotId = _botId
                };

                short unsuccessCounter = 0;

                while (!cancellationToken.IsCancellationRequested || _keepAliveEvent.IsSet)
                {
                    var response =
                        await InnerSend<KeepAliveNotificationRequest, KeepAliveNotificationResponse>(request, "KeepAlive",
                            cancellationToken);

                    if (!response.IsSuccess)
                    {
                        ++unsuccessCounter;
                        if (unsuccessCounter == MaxRetryCount)
                        {
                            // TODO: log

                            break;
                        }
                    }

                    Thread.Sleep(2 * PausePeriod);
                }
            }, cancellationToken);

            if (_keepAliveTask.IsFaulted)
                throw new BotException($"{nameof(KeepAlive)} exception: {_keepAliveTask.Exception?.Message}",
                    _keepAliveTask.Exception);
        }

        private void GetRequiredStatus(CancellationToken cancellationToken)
        {
            if (_getRequiredStatusEventTask != default)
                return;

            _getRequiredStatusEvent.Set();
            _getRequiredStatusEventTask = Task.Run(async () =>
            {
                if (_subBots == null || !_subBots.Any())
                    _subBots = _serviceProvider.GetServices<IBot>().ToArray();

                var request = new GetRequiredStatusFromServerRequest()
                {
                    BotId = _botId
                };

                short unsuccessCounter = 0;
                while (!cancellationToken.IsCancellationRequested || _getRequiredStatusEvent.IsSet)
                {
                    var response =
                        await InnerSend<GetRequiredStatusFromServerRequest, GetRequiredStatusFromServerResponse>(request, "GetRequiredStatus",
                            cancellationToken);

                    if (!response.IsSuccess)
                    {
                        ++unsuccessCounter;
                        if (unsuccessCounter == MaxRetryCount)
                        {
                            // TODO: log

                            break;
                        }
                    }
                    else
                    {
                        switch (response.Status)
                        {
                            case BotStatus.Active:
                                foreach (var bot in _subBots)
                                    await bot.StartBotAsync(StartBotRequest.GetInstance(), cancellationToken);
                                break;
                            case BotStatus.NonActive:
                                foreach (var bot in _subBots)
                                    await bot.StopBotAsync(StopBotRequest.GetInstance(), cancellationToken);

                                break;
                            case BotStatus.Unknown:
                            case null:
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }

                    Thread.Sleep(PausePeriod);
                }
            }, cancellationToken);

            if (_getRequiredStatusEventTask.IsFaulted)
                throw new BotException($"{nameof(GetRequiredStatus)} exception: {_keepAliveTask.Exception?.Message}",
                    _keepAliveTask.Exception);
        }


        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _keepAliveEvent.Reset();
            _getRequiredStatusEvent.Reset();
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
                throw new BotException($"Error sending request to a Botticelli server!");

            var responseJson = await httpResponse.Content.ReadAsStringAsync(cancellationToken);

            return JsonConvert.DeserializeObject<TResp>(responseJson);
        }
    }
}
