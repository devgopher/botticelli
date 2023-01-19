using Botticelli.DataLayer.Context;
using Botticelli.Shared.ValueObjects;

namespace Botticelli.DataLayer;

/// <summary>
///     A storage for messages
/// </summary>
public class MessageStorage : DbStorage<Chat, string>
{
    public MessageStorage(BotticelliContext context) : base(context)
    {
    }
}