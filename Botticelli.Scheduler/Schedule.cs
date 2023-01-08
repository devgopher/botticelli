using Botticelli.Scheduler.Interfaces;

namespace Botticelli.Scheduler
{
    public class Schedule : ISchedule
    {
        public string? Name { get; set; }
        public string? Cron { get; set; }
    }
}