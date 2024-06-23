namespace Botticelli.LoadTests.Shared;

public interface ITest<TResult>
{
    public Guid Id { get; }
    public string Name { get; }
    public Task<ITestResult> Run(ITestInput input);
    public TestSettings Settings { get; }
    public DateTime Started { get; set; }
    public DateTime Finished { get; set; }
}