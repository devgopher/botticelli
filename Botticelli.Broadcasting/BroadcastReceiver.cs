using Botticelli.Broadcasting.Dal;
using Botticelli.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Botticelli.Broadcasting;

public class BroadcastReceiver<TBot> : IHostedService
where TBot : class, IBot<TBot>
{
    private readonly IServiceScope _scope;
    private readonly BroadcastingContext _context;
    private readonly TBot _bot;
    
    public BroadcastReceiver(IServiceProvider serviceProvider)
    {
        _scope = serviceProvider.CreateScope();
        _bot = _scope.ServiceProvider.GetRequiredService<TBot>();
        _context = _scope.ServiceProvider.GetRequiredService<BroadcastingContext>();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        // TODO : polls admin API for new messages to send to our chats and adds them to a MessageCache/MessageStatus
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _scope.Dispose();
        
        return Task.CompletedTask;
    }
}