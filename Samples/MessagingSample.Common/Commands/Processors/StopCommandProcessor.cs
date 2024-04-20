using Botticelli.Client.Analytics;
using Botticelli.Framework.Commands.Processors;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Scheduler.Hangfire;
using Botticelli.Scheduler.Interfaces;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.ValueObjects;
using Microsoft.Extensions.Logging;

namespace MessagingSample.Common.Commands.Processors;

public class StopCommandProcessor : CommandProcessor<StopCommand>
{
    private readonly IJobManager _jobManager;
    
    public StopCommandProcessor(ILogger<StopCommandProcessor> logger,
        ICommandValidator<StopCommand> validator,
        MetricsProcessor metricsProcessor, IJobManager jobManager)
        : base(logger, validator, metricsProcessor)
    {
        _jobManager = jobManager;
    }

    protected override async Task InnerProcessContact(Message message, string argsString, CancellationToken token)
    {
    }

    protected override async Task InnerProcessPoll(Message message, string argsString, CancellationToken token)
    {
    }

    protected override async Task InnerProcessLocation(Message message, string argsString, CancellationToken token)
    {
    }


    protected override async Task InnerProcess(Message message, string args, CancellationToken token)
    {
        _jobManager.RemoveAllJobs();

        var farewellMessageRequest = new SendMessageRequest(Guid.NewGuid().ToString())
        {
            Message = new Message
            {
                Uid = Guid.NewGuid().ToString(),
                ChatIds = message.ChatIds,
                Body = "Bot stopped..."
            }
        };

        await Bot.SendMessageAsync(farewellMessageRequest, token);
    }
}