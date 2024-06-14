namespace Botticelli.LoadTests.Shared;

/// <summary>
/// General step interface
/// </summary>
public interface IStep
{
    public TimeSpan Elapsed { get; set; }
    
    public Task<IStepResult> Go(IStepInput input);
}