namespace Botticelli.LoadTests.Shared;

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

/// <summary>
/// General step interface
/// </summary>
public interface IStep
{
    public TimeSpan Elapsed { get; set; }

    public Task<IStepResult> Go(IStepInput input);
}