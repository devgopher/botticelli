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
/// BotAction class incapsulates logic for sending messages using some schedule
/// with some reliability parameters
/// </summary>
public class BotAction : IJob
{
    private readonly AsyncRetryPolicy<SendMessageResponse> _sendOkPolicy;
    private readonly AsyncRetryPolicy<SendMessageResponse> _sendExceptionPolicy;
    private readonly AsyncPolicyWrap<SendMessageResponse> _sendPolicy;


    public BotAction(Reliability reliability,
                     Message message,
                     IBot bot,
                     string jobName,
                     string jobDescription)
    {
        Reliability = reliability;
        Message = message;
        Bot = bot;
        JobName = jobName;
        JobDescription = jobDescription;

        if (Reliability.IsEnabled)
        {
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
    }

    public Message Message { get; }
    public string JobName { get; }
    public string JobDescription { get; }
    private IBot Bot { get; }
    private Reliability Reliability { get; }

    public async Task RunAsync(CancellationToken token)
    {
        var request = new SendMessageRequest(Guid.NewGuid().ToString());

        if (!Reliability.IsEnabled)
        {
            await Bot.SendMessageAsync(request, token);

            return;
        }

        await _sendPolicy
                .ExecuteAndCaptureAsync(async ct => await Bot.SendMessageAsync(request, ct), token);
    }
}