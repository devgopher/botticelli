namespace Botticelli.LoadTests.Shared;

public interface ITestResult
{
    public Guid TestId { get; }
    public TimeSpan ExecutionTime { get; }
    public Dictionary<string, object> AdditionalFields { get; }
}

