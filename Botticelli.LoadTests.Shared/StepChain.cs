using System.Diagnostics;

namespace Botticelli.LoadTests.Shared;

public class StepChain
{
    private readonly IList<IStep> _steps;
    private readonly Stopwatch _stopwatch = new();

    public StepChain(IList<IStep> steps) => _steps = steps;
    

    public async Task<(IStepResult result, TimeSpan elapsed)> Run(IStepInput input)
    {
        IStepResult finalResult;
        try
        {
            if (_steps == null || !_steps.Any()) throw new InvalidDataException("No steps!");

            if (!(input.GetType().GetGenericArguments().First() == _steps.FirstOrDefault()?.GetType()))
            {
                throw new InvalidDataException($"A type of input data should be: {input.GetType().GetGenericArguments().First().Name}," +
                                               $" but we've: {_steps.FirstOrDefault()?.GetType()?.Name}!");
            }


            _stopwatch.Start();
            var stepInput = input;
            

            foreach (var step in _steps)
            {
                if (step == _steps.LastOrDefault())
                {
                    finalResult = await step.Go(stepInput);

                    break;
                }

                stepInput = await step.Go(stepInput);
            }
        }
        catch (Exception ex)
        {
            finalResult = new ErrorResult()
            {
                Error = ex.Message
            };
        }
        finally
        {
            _stopwatch.Stop();

            return (finalResult, _stopwatch.Elapsed);
        }
    }
}