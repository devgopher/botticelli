using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.LoadTests.Shared;

public class StepChainBuilder
{
    private readonly List<IStep> _steps = new();

    public StepChainBuilder(IServiceCollection services)
    {
        
    }

    public StepChainBuilder Add(IStep step)
    {
        _steps.Add(step);

        return this;
    }

    public StepChain Build() => new(_steps);
}