using Botticelli.LoadTests.Receiver.Controller;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.LoadTests.Receiver.Extensions;

public static class ServiceCollectionExtensions
{
    public static IHostApplicationBuilder AddLoadTesting<TBot>(this IHostApplicationBuilder builder)
    {
        var loadAsm = typeof(LoadTestController).Assembly;
        builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(loadAsm));
        builder.Services.AddScoped<ILoadTestGate, LoadTestGate>();

        return builder;
    }
}