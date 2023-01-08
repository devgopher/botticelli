using Botticelli.DataLayer.Context;
using Botticelli.Shared.ValueObjects;

namespace Botticelli.DataLayer
{
    /// <summary>
    /// A storage for chats
    /// </summary>
    public class ChatStorage : DbStorage<Chat, string>
    {
        public ChatStorage(ChatContext context) : base(context)
        {
        }
    }
}
