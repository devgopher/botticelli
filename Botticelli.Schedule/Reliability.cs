namespace Botticelli.Scheduler;

public class Reliability
{
    public bool IsEnabled { get; }
    public int MaxTries { get;  }
    public bool IsExponential { get; }
    public TimeSpan Delay { get;  }
}