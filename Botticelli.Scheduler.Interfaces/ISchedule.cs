namespace Botticelli.Scheduler.Interfaces
{
    public interface ISchedule
    {
        public string Name { get; }
        public string Cron { get; }
    }
}