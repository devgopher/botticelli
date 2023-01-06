namespace Botticelli.Shared.API.Client.Commands;

/// <summary>
///     Bot command parameter with name and it's value
/// </summary>
public struct BotCommandParameter
{
    public string Name { get; set; }
    public object Value { get; set; }
}