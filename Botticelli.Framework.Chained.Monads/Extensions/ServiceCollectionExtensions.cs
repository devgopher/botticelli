using Botticelli.Framework.Chained.Monads.Commands;
using Botticelli.Framework.Chained.Monads.Commands.Processors;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Framework.Controls.Parsers;
using Botticelli.Framework.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.Framework.Chained.Monads.Extensions;

public static class ServiceCollectionExtensions
{
    public static CommandAddServices<TCommand> AddMonadsChain<TCommand, TValidator>(this CommandAddServices<TCommand> commandAddServices,
                                                                                    IServiceCollection services,
                                                                                    Action<ChainBuilder<TCommand>> chainBuilderOptions)
            where TCommand : class, IChainCommand, new()
            where TValidator : class, ICommandValidator<TCommand>
    {
        var chainBuilder = new ChainBuilder<TCommand>(services);
        chainBuilderOptions(chainBuilder);

        var runner = chainBuilder.Build();

        services.AddSingleton(_ => runner);

        return commandAddServices.AddProcessor<ChainRunProcessor<TCommand>>()
                                 .AddValidator<TValidator>();
    }

    public static CommandAddServices<TCommand> AddMonadsChain<TCommand, TValidator, TReplyMarkup, TLayoutSupplier>(this CommandAddServices<TCommand> commandAddServices,
                                                                                                                   IServiceCollection services,
                                                                                                                   Action<ChainBuilder<TCommand>> chainBuilderOptions)
            where TCommand : class, IChainCommand, new()
            where TValidator : class, ICommandValidator<TCommand>
            where TLayoutSupplier : class, ILayoutSupplier<TReplyMarkup>
    {
        var chainBuilder = new ChainBuilder<TCommand>(services);
        chainBuilderOptions(chainBuilder);

        var runner = chainBuilder.Build();

        services.AddSingleton(_ => runner)
                .AddSingleton<ILayoutSupplier<TReplyMarkup>, TLayoutSupplier>();

        return commandAddServices.AddProcessor<ChainRunProcessor<TCommand>>()
                                 .AddValidator<TValidator>();
    }
}