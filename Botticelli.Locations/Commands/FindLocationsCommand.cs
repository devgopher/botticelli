using Botticelli.Framework.Commands;

namespace Botticelli.Locations.Commands;

public class FindLocationsCommand : ICommand
{
    public Guid Id { get; }
}