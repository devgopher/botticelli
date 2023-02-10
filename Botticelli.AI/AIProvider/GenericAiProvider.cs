using Botticelli.AI.Exceptions;
using Botticelli.AI.Extensions;
using Botticelli.AI.Message;
using Botticelli.Interfaces;
using Botticelli.Shared.API;
using Botticelli.Shared.API.Client.Requests;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;

namespace Botticelli.AI.AIProvider
{
    public abstract class GenericAiProvider : IAiProvider
    {
        private readonly IOptionsMonitor<AISettings> _settings;
        private readonly IHttpClientFactory _factory;
        private readonly ILogger _logger;
        private readonly IBot _bot;

        public GenericAiProvider(IOptionsMonitor<AISettings> settings, 
                                 IHttpClientFactory factory,
                                 ILogger logger,
                                 IBot bot)
        {
            _settings = settings;
            _factory = factory;
            _logger = logger;
            _bot = bot;
            
        }

        public abstract Task SendAsync(AIMessage message, CancellationToken token);

        public virtual async Task ProcessAiResponses(CancellationToken token)
        {
            _logger.LogInformation($"{nameof(ProcessAiResponses)}() started...");   

            await Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    var response = await InnerReceiveResponse();

                    if (response != default)
                    {
                        // TODO: reliability!!
                        var botResponse = await _bot.SendMessageAsync(new SendMessageRequest(Guid.NewGuid().ToString())
                                                                      {
                                                                          Message = response
                                                                      },
                                                                      token);
                        
                        _logger.LogTrace($"Got response from a bot: {botResponse.MessageUid}");

                        if (botResponse.MessageSentStatus != MessageSentStatus.OK)
                        {
                            _logger.LogError($"Bot response is: {botResponse.MessageSentStatus}, " + 
                                             $"{botResponse.TechMessage}. It's not successful!");
                        }
                        //throw new AiException($"Bot response is: {botResponse.MessageSentStatus}, " +
                        //                      $"{botResponse.TechMessage}. It's not successful!")
                    }

                    Thread.Sleep(200);
                }
            }, token);

            _logger.LogInformation($"{nameof(ProcessAiResponses)}() stopped");
        }

        protected abstract Task<AIMessage?> InnerReceiveResponse();
    }
}