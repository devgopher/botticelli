namespace Botticelli.Scheduler
{
    public interface IJob
    {
        public string JobName { get; }
        public string JobDescription { get; }
        public Task RunAsync(CancellationToken token);
    }
}
