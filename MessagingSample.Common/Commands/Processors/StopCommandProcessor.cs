using Botticelli.Framework.Commands.Processors;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Scheduler;
using Botticelli.Shared.ValueObjects;
using Microsoft.Extensions.Logging;

namespace MessagingSample.Common.Commands.Processors;

public class StopCommandProcessor : CommandProcessor<StopCommand>
{
    public StopCommandProcessor(ILogger<StopCommandProcessor> logger,
        ICommandValidator<StopCommand> validator)
        : base(logger, validator)
    {
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
        JobManager.RemoveAllJobs();
    }
}