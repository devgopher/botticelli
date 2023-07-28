using LiteDB;

namespace Botticelli.Shared.ValueObjects
{
    public class BotContext
    {
        /// <summary>
        /// Botticelli bot id
        /// </summary>
        [BsonId]
        public string BotId { get; set; }
        
        /// <summary>
        /// Bot key in a messenger
        /// </summary>
        public string BotKey { get; set; }

        /// <summary>
        /// Additional info
        /// </summary>
        public Dictionary<string, string> Items { get; set; }
    }
}
