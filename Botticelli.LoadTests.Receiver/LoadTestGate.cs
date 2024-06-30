using System.Diagnostics;
using Botticelli.Framework.Commands.Processors;
using Botticelli.LoadTests.Receiver.Result;
using Botticelli.Shared.ValueObjects;

namespace Botticelli.LoadTests.Receiver;

public class LoadTestGate : ILoadTestGate
{
    private readonly CommandProcessorFactory _processors;

    public LoadTestGate(CommandProcessorFactory processors) 
        => _processors = processors;

    public Task ThrowCommand(string command, string args, CancellationToken token)
    {
        var message = new Message
        {
            Uid = Guid.NewGuid().ToString(),
            ChatIdInnerIdLinks = new Dictionary<string, List<string>>(),
            ChatIds = [Random.Shared.Next().ToString()],
            Subject = string.Empty,
            Body = $"/{command} {args}",
            Attachments = null,
            From = null,
            ForwardedFrom = null,
            Contact = null,
            Poll = null,
            ReplyToMessageUid = null,
            Location = null,
            CallbackData = null,
            CreatedAt = DateTime.Now,
            LastModifiedAt = DateTime.Now
        };

        return Task.Factory.StartNew(() => _processors.Get(command)?.ProcessAsync(message, token), token);
    }

    public async Task<CommandResult> WaitForExecution(Task task, TimeSpan timeout, CancellationToken token)
    {
        var stopwatch = Stopwatch.StartNew();

        try
        {
            await task.WaitAsync(timeout, token);
            
            return new CommandResult
            {
                ResultMessage = "Success",
                Duration = stopwatch.Elapsed,
                IsSuccess = true
            };
        }
        catch (Exception ex)
        {
            return new CommandResult
            {
                ResultMessage = $"Error: {ex.Message}",
                Duration = stopwatch.Elapsed,
                IsSuccess = false
            };
        }
        finally
        {
            stopwatch.Stop();
        }
    }
}