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

/// <summary>
/// A step interface
/// </summary>
/// <typeparam name="TStepInput"></typeparam>
/// <typeparam name="TStepResult"></typeparam>
public interface IStep<in TStepInput, TStepResult> : IStep
where TStepInput : IStepInput
where TStepResult : IStepResult
{
    public Task<TStepResult> Go(TStepInput? input);
}