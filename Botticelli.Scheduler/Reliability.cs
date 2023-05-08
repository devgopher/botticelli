namespace Botticelli.Scheduler;

public class Reliability
{
    public bool IsEnabled { get; set;  }
    public int MaxTries { get; set; }
    public bool IsExponential { get; set; }
    public TimeSpan Delay { get; set; }
}