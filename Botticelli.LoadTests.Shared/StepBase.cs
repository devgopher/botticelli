using System.Diagnostics;

namespace Botticelli.LoadTests.Shared;

/// <summary>
/// A basic step
/// </summary>
/// <typeparam name="TStepInput"></typeparam>
/// <typeparam name="TStepResult"></typeparam>
public abstract class StepBase<TStepInput, TStepResult> : IStep<TStepInput, TStepResult> 
        where TStepInput : class, IStepInput
        where TStepResult : class, IStepResult
{
    private readonly TestSettings _settings;

    public StepBase(TestSettings settings) => _settings = settings;

    public async Task<IStepResult> Go(IStepInput input) 
        => await Go(input as TStepInput);

    public Task<TStepResult> Go(TStepInput? input)
    {
        try
        {
            _stopwatch.Start();
            var task = InnerGo(input);

            task.Wait((int) _settings.TestRunTimeout.TotalMilliseconds);

            return Task.FromResult(task.Result);
        }
        finally
        {
            _stopwatch.Stop();
            Elapsed = _stopwatch.Elapsed;
        }
    }


    protected abstract Task<TStepResult> InnerGo(TStepInput? input);
    private readonly Stopwatch _stopwatch = new();
    public TimeSpan Elapsed { get; set; } = TimeSpan.Zero;

}