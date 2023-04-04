using Botticelli.Framework.Commands.Processors;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Shared.ValueObjects;
using ViberBotSample.Commands;

namespace ViberBotSample;

public class SampleCommandProcessor : CommandProcessor<SampleCommand>
{
    public SampleCommandProcessor(ILogger<SampleCommandProcessor> logger,
                                  ICommandValidator<SampleCommand> validator) : base(logger, validator)
    {
    }

    protected override async Task InnerProcess(Message message, string args, CancellationToken token)
        => Console.WriteLine("Command received!!");
}