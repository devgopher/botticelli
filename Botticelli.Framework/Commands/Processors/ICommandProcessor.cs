namespace Botticelli.Framework.Commands.Processors
{
    public interface ICommandProcessor
    {
        Task ProcessAsync(long chatId, CancellationToken token, params string[] args);
    }
}
