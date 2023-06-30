using Botticelli.Bus.ZeroMQTests.Handler;

namespace Botticelli.Bus.ZeroMQTests.Mocks;

internal class ServiceProviderMock : IServiceProvider
{
    public object? GetService(Type serviceType)
    {
        if (serviceType == typeof(TestHandler)) return new TestHandler();

        throw new NotSupportedException($"Type {serviceType.Name} is not supported!");
    }
}