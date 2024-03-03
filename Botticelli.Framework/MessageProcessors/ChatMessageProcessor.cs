using System.Text.RegularExpressions;
using Botticelli.Analytics.Shared.Metrics;
using Botticelli.BotBase.Utils;
using Botticelli.Client.Analytics;
using Botticelli.Framework.Commands.Processors;
using Botticelli.Interfaces;
using Botticelli.Shared.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Botticelli.Framework.MessageProcessors;

public sealed partial class ChatMessageProcessor : IClientMessageProcessor
{
    private const string SimpleCommandPattern = @"\/([a-zA-Z0-9]*)$";
    private const string ArgsCommandPattern = @"\/([a-zA-Z0-9]*) (.*)";

    private readonly CommandProcessorFactory _cpFactory;
    private readonly ILogger<ChatMessageProcessor> _logger;
    private readonly MetricsProcessor _metrics;

    public ChatMessageProcessor(ILogger<ChatMessageProcessor> logger,
        CommandProcessorFactory cpFactory,
        MetricsProcessor metrics)
    {
        _logger = logger;
        _cpFactory = cpFactory;
        _metrics = metrics;
    }

    public async Task ProcessAsync(Message message, CancellationToken token)
    {
        _logger.LogDebug($"{nameof(ProcessAsync)}({message.Uid}) started...");

        _metrics.Process(MetricNames.MessageReceived, BotDataUtils.GetBotId()!);

        try
        {
            string command = string.Empty;

            if (SimpleCommandRegex().IsMatch(message.Body!))
            {
                var match = SimpleCommandRegex().Matches(message.Body)
                                                .FirstOrDefault();

                if (match == default) return;

                command = match.Groups[1]
                    .Value;
            }
            else if (ArgsCommandRegex().IsMatch(message.Body))
            {
                var match = ArgsCommandRegex().Matches(message.Body)
                                              .FirstOrDefault();

                command = match!.Groups[1].Value;

                var argsString = match.Groups[2].Value;

                if (!string.IsNullOrWhiteSpace(argsString)) 
                    argsString.Split(" ");
            }
            
            if (command != string.Empty)
                await _cpFactory.Get(command)!
                                .ProcessAsync(message, token);

            _logger.LogDebug($"{nameof(ProcessAsync)}({message.Uid}) finished...");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in {nameof(ChatMessageProcessor)}: {ex.Message}");
        }
    }

    public void SetBot(IBot bot)
    {
    }

    public void SetServiceProvider(IServiceProvider sp)
    {
        throw new NotImplementedException();
    }

    [GeneratedRegex("\\/([a-zA-Z0-9]*) (.*)")]
    private static partial Regex ArgsCommandRegex();
    
    [GeneratedRegex("\\/([a-zA-Z0-9]*)$")]
    private static partial Regex SimpleCommandRegex();
}