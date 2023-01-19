using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.Framework.Commands.Processors
{
    public class CommandProcessorFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public CommandProcessorFactory(IServiceProvider serviceProvider)
            => _serviceProvider = serviceProvider;

        public ICommandProcessor Get(string command)
        {
            var canonized = "";

            if (string.IsNullOrEmpty(command))
                throw new ArgumentNullException(nameof(command), "Can't be null or empty!");

            if (command.Length == 1)
                canonized = command.ToUpperInvariant();
            else
                canonized = $"{command[..1].ToUpperInvariant()}{command[1..].ToLowerInvariant()}";

            switch (canonized)
            {
                default:
                    return _serviceProvider.GetRequiredService<CommandProcessor<Unknown>>();
            }

        }
    }
}
