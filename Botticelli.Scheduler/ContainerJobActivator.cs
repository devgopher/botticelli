using Hangfire;
using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.Scheduler;

public class ContainerJobActivator : JobActivator
{
    private readonly IServiceCollection _services;

    public ContainerJobActivator(IServiceCollection services) => _services = services;

    public override object ActivateJob(Type type)
    {
        var realTypeDescriptor = _services
                                 .AsEnumerable()
                                 .FirstOrDefault(s => s.ServiceType.IsInterface &&
                                                      s.ServiceType
                                                       .FullName
                                                       .ToLowerInvariant()
                                                       .Contains(type.Name.ToLowerInvariant()));

        return _services.BuildServiceProvider().GetRequiredService(realTypeDescriptor.ServiceType);
    }
}