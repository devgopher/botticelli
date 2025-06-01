using System.Text.Json;
using System.Text.Json.Serialization;
using Botticelli.Interfaces;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Botticelli.Broadcasting.Dal;

/// <summary>
///     Represents a service that sends messages from the database to a bot.
///     This class retrieves messages and updates their statuses after sending.
/// </summary>
/// <typeparam name="TBot">The type of the bot that implements the IBot interface.</typeparam>
public class BroadcastSender<TBot> : IHostedService, IDisposable
        where TBot : class, IBot<TBot>
{
    private readonly TBot _bot;
    private readonly BroadcastingContext _context;
    private readonly IServiceScope _scope;
    private CancellationTokenSource _cancellationTokenSource;
    private Task _executingTask;

    /// <summary>
    ///     Initializes a new instance of the <see cref="BroadcastSender{TBot}" /> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider used to create a scope and resolve dependencies.</param>
    public BroadcastSender(IServiceProvider serviceProvider)
    {
        _scope = serviceProvider.CreateScope();
        _bot = _scope.ServiceProvider.GetRequiredService<TBot>();
        _context = _scope.ServiceProvider.GetRequiredService<BroadcastingContext>();
    }

    /// <summary>
    ///     Disposes of the resources used by the broadcast sender.
    /// </summary>
    public void Dispose()
    {
        _scope.Dispose();
        _cancellationTokenSource?.Dispose();
    }

    /// <summary>
    ///     Starts the broadcast sender service.
    ///     This method retrieves messages from the database and sends them to the bot in a loop.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to signal the operation's cancellation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        _executingTask = Task.Run(() => ExecuteAsync(_cancellationTokenSource.Token), cancellationToken);

        return Task.CompletedTask;
    }

    /// <summary>
    ///     Stops the broadcast sender service.
    ///     This method disposes of the service scope and cancels the executing task.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to signal the operation's cancellation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task StopAsync(CancellationToken cancellationToken)
    {
        _cancellationTokenSource.Cancel();

        return _executingTask;
    }

    /// <summary>
    ///     The main execution loop that retrieves and sends messages.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to signal the operation's cancellation.</param>
    private async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            // Retrieve messages from the database
            var messageStatuses = await _context.MessageStatuses
                                                .Where(ms => !ms.IsSent) // Get messages that have not been sent
                                                .ToListAsync(cancellationToken).ConfigureAwait(false);
            
            foreach (var messageStatus in messageStatuses)
            {
                var messageSource = _context.MessageCaches.FirstOrDefault(m => m.Id == messageStatus.MessageId);
                
                if (messageSource == null)
                    continue;
                
                var deserializedMessage = JsonSerializer.Deserialize<Message>(messageSource.SerializedMessageObject);
                
                if (deserializedMessage == null)
                    continue;
                
                var request = new SendMessageRequest
                {
                    Message = deserializedMessage
                };

                // Send the message to the bot
                var sendResult = await _bot.SendMessageAsync(request, cancellationToken).ConfigureAwait(false);
                
                // Update the message status
                messageStatus.IsSent = sendResult.Message != null;
                messageStatus.SentDate = messageStatus.IsSent ? DateTime.UtcNow : null;

                // Save changes to the database
                _context.MessageStatuses.Update(messageStatus);
            }

            await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            // Wait for a specified interval before checking for new messages again
            await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken).ConfigureAwait(false); // Adjust the delay as needed
        }
    }
}