using System.Reflection;
using Botticelli.Client.Analytics;
using Botticelli.Framework.Commands.Processors;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Framework.Controls.Parsers;
using Botticelli.Framework.SendOptions;
using Botticelli.Scheduler.Interfaces;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.ValueObjects;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace MessagingSample.Common.Commands.Processors;

public class StopCommandProcessor<TReplyMarkup> : CommandProcessor<StopCommand>
        where TReplyMarkup : class
{
    private readonly IJobManager _jobManager;
    private SendOptionsBuilder<TReplyMarkup>? _options;

    public StopCommandProcessor(ILogger<StopCommandProcessor<TReplyMarkup>> logger,
        ICommandValidator<StopCommand> commandValidator,
        IJobManager jobManager,
        ILayoutSupplier<TReplyMarkup> layoutSupplier,
        ILayoutParser layoutParser,
        IValidator<Message> messageValidator)
        : base(logger,
            commandValidator,
            messageValidator)
    {
        _jobManager = jobManager;
        Init(layoutSupplier, layoutParser);
    }
    
    public StopCommandProcessor(ILogger<StopCommandProcessor<TReplyMarkup>> logger,
        ICommandValidator<StopCommand> commandValidator,
        IJobManager jobManager,
        ILayoutSupplier<TReplyMarkup> layoutSupplier,
        ILayoutParser layoutParser,
        IValidator<Message> messageValidator,
        MetricsProcessor? metricsProcessor)
            : base(logger,
                   commandValidator,
                   messageValidator, 
                   metricsProcessor)
    {
        _jobManager = jobManager;
        Init(layoutSupplier, layoutParser);
    }

    private void Init(ILayoutSupplier<TReplyMarkup> layoutSupplier, ILayoutParser layoutParser)
    {
        var location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;
        var responseLayout = layoutParser.ParseFromFile(Path.Combine(location, "start_layout.json"));
        var responseMarkup = layoutSupplier.GetMarkup(responseLayout);

        _options = SendOptionsBuilder<TReplyMarkup>.CreateBuilder(responseMarkup);
    }

    protected override Task InnerProcessContact(Message message, CancellationToken token)
    {
        return Task.CompletedTask;
    }

    protected override Task InnerProcessPoll(Message message, CancellationToken token)
    {
        return Task.CompletedTask;
    }

    protected override Task InnerProcessLocation(Message message, CancellationToken token)
    {
        return Task.CompletedTask;
    }


    protected override async Task InnerProcess(Message message, CancellationToken token)
    {
        _jobManager.RemoveAllJobs();

        var farewellMessageRequest = new SendMessageRequest
        {
            Message = new Message
            {
                Uid = Guid.NewGuid().ToString(),
                ChatIds = message.ChatIds,
                Body = "Bot stopped..."
            }
        };

        await Bot.SendMessageAsync(farewellMessageRequest, _options, token);
    }
}