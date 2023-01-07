using Botticelli.DataLayer.Context;
using Botticelli.Shared.ValueObjects;

namespace Botticelli.DataLayer
{
    public class ChatStorage : DbStorage<Chat, string>
    {
        public ChatStorage(ChatContext context) : base(context)
        {
        }
    }
}
