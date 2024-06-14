using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.LoadTests.Shared;

public class StepChainBuilder
{
    private readonly StepChain _chain = new StepChain(new List<IStep>(5));

    public StepChainBuilder(IServiceCollection services)
    {
        
    }

    public StepChainBuilder Add()
    {
        
    }
}