using Botticelli.Interfaces;

namespace Botticelli.Scheduler.Interfaces;

public interface IScheduledJob<TBot>
        where TBot : IBot<TBot>
{
    public Task Execute(TBot bot);
}