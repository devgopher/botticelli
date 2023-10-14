using System.Text.RegularExpressions;
using Botticelli.Analytics.Shared.Metrics;
using Botticelli.Client.Analytics;
using Botticelli.Framework.Commands.Processors;
using Botticelli.Interfaces;
using Botticelli.Shared.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Botticelli.Framework.MessageProcessors;

public sealed class ChatMessageProcessor : IClientMessageProcessor
{
    private const string SimpleCommandPattern = @"\/([a-zA-Z0-9]*)$";
    private const string ArgsCommandPattern = @"\/([a-zA-Z0-9]*) (.*)";

    private readonly CommandProcessorFactory _cpFactory;
    private readonly MetricsProcessor _metrics;
    private readonly ILogger<ChatMessageProcessor> _logger;

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

        _metrics.Process(MetricNames.MessageReceived);

        try
        {
            string command;

            if (Regex.IsMatch(message.Body, SimpleCommandPattern))
            {
                var match = Regex.Matches(message.Body, SimpleCommandPattern)
                                 .FirstOrDefault();

                if (match == default) return;

                command = match.Groups[1]
                               .Value;

                await _cpFactory.Get(command)
                                .ProcessAsync(message, token);
            }
            else if (Regex.IsMatch(message.Body, ArgsCommandPattern))
            {
                var match = Regex.Matches(message.Body, ArgsCommandPattern)
                                 .FirstOrDefault();

                command = match.Groups[1].Value;

                var argsString = match.Groups[2].Value;

                var args = Array.Empty<string>();

                if (!string.IsNullOrWhiteSpace(argsString)) args = argsString.Split(" ");

                await _cpFactory.Get(command)
                                .ProcessAsync(message, token);
            }

            _logger.LogDebug($"{nameof(ProcessAsync)}({message.Uid}) finished...");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in {nameof(ChatMessageProcessor)}: {ex.Message}");
        }
    }

    public void AddBot(IBot bot)
    {
    }

    public void SetServiceProvider(IServiceProvider sp)
    {
        throw new NotImplementedException();
    }
}