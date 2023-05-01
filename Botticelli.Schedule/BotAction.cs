using Botticelli.Interfaces;
using Botticelli.Shared.API;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;
using Botticelli.Shared.ValueObjects;
using Polly;
using Polly.Retry;
using Polly.Wrap;

namespace Botticelli.Scheduler;

/// <summary>
///     BotAction class incapsulates logic for sending messages using some schedule
///     with some reliability parameters
/// </summary>
public class BotAction<TBot> : IJob
where  TBot : IBot
{
    private AsyncRetryPolicy<SendMessageResponse> _sendExceptionPolicy;
    private AsyncRetryPolicy<SendMessageResponse> _sendOkPolicy;
    private AsyncPolicyWrap<SendMessageResponse> _sendPolicy;
    //private readonly ILogger _logger;

    private void AcceptParams()
    {
        if (!Reliability.IsEnabled) return;

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

    public Message Message { get; set; }
    public IBot Bot { get; set; }
    public Reliability Reliability { get; set; }
    public string JobName { get; set; }
    public string JobDescription { get; set; }

    public async Task RunAsync(CancellationToken token)
    {
        try
        {
            AcceptParams();
            //_logger.LogDebug($"{nameof(RunAsync)}(): started...");
            var request = new SendMessageRequest(Guid.NewGuid().ToString());

            if (!Reliability.IsEnabled)
            {
                //_logger.LogDebug($"{nameof(RunAsync)}(): no reliability");
                var response = await Bot.SendMessageAsync(request, token);

                //_logger.LogTrace($"{nameof(RunAsync)}(): send status={response.MessageSentStatus}");

                return;
            }

            //_logger.LogDebug($"{nameof(RunAsync)}(): reliability");
            var result = await _sendPolicy
                    .ExecuteAndCaptureAsync(async ct => await Bot.SendMessageAsync(request, ct), token);

            //_logger.LogTrace($"{nameof(RunAsync)}(): send status={result?.Result?.MessageSentStatus}");
        }
        catch (Exception ex)
        {
            //_logger.LogError(ex, $"{nameof(RunAsync)}() error: {ex.Message}!");
        }
    }
}