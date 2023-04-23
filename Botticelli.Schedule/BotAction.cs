using Botticelli.Interfaces;
using Botticelli.Shared.API;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;
using Botticelli.Shared.ValueObjects;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using Polly.Wrap;

namespace Botticelli.Scheduler;

/// <summary>
///     BotAction class incapsulates logic for sending messages using some schedule
///     with some reliability parameters
/// </summary>
public class BotAction<TBot> : IJob
where  TBot : IBot<TBot>
{
    private readonly AsyncRetryPolicy<SendMessageResponse> _sendExceptionPolicy;
    private readonly AsyncRetryPolicy<SendMessageResponse> _sendOkPolicy;
    private readonly AsyncPolicyWrap<SendMessageResponse> _sendPolicy;
    private readonly ILogger _logger;

    public BotAction(IServiceProvider sp,
                     Reliability reliability,
                     Message message,
                     string jobName,
                     string jobDescription,
                     ILogger logger)
    {
        Reliability = reliability;
        Message = message;
        Bot = sp.GetRequiredService<TBot>();
        JobName = jobName;
        JobDescription = jobDescription;
        _logger = logger;

        if (!Reliability.IsEnabled) 
            return;

        _sendOkPolicy = Policy<SendMessageResponse>
                        .HandleResult(resp => resp.MessageSentStatus != MessageSentStatus.Ok)
                        .WaitAndRetryAsync(Reliability.MaxTries,
                                           n => Reliability.IsExponential ?
                                                   TimeSpan.FromSeconds(n * Math.Exp(Reliability.Delay.TotalSeconds)) :
                                                   Reliability.Delay);
        _sendExceptionPolicy = Policy<SendMessageResponse>
                               .Handle<Exception>()
                               .WaitAndRetryAsync(Reliability.MaxTries,
                                                  n => Reliability.IsExponential ?
                                                          TimeSpan.FromSeconds(n * Math.Exp(Reliability.Delay.TotalSeconds)) :
                                                          Reliability.Delay);

        _sendPolicy = _sendExceptionPolicy.WrapAsync(_sendOkPolicy);
    }

    public Message Message { get; }
    private IBot Bot { get; }
    private Reliability Reliability { get; }
    public string JobName { get; }
    public string JobDescription { get; }

    public async Task RunAsync(CancellationToken token)
    {
        try
        {
            _logger.LogDebug($"{nameof(RunAsync)}(): started...");
            var request = new SendMessageRequest(Guid.NewGuid().ToString());

            if (!Reliability.IsEnabled)
            {
                _logger.LogDebug($"{nameof(RunAsync)}(): no reliability");
                var response = await Bot.SendMessageAsync(request, token);

                _logger.LogTrace($"{nameof(RunAsync)}(): send status={response.MessageSentStatus}");

                return;
            }

            _logger.LogDebug($"{nameof(RunAsync)}(): reliability");
            var result = await _sendPolicy
                    .ExecuteAndCaptureAsync(async ct => await Bot.SendMessageAsync(request, ct), token);

            _logger.LogTrace($"{nameof(RunAsync)}(): send status={result?.Result?.MessageSentStatus}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"{nameof(RunAsync)}() error: {ex.Message}!");
        }
    }
}