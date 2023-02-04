using LiteDB;

namespace BotDataSecureStorage.Entities
{
    /// <summary>
    /// Bot keys
    /// </summary>
    public class BotKey
    {
        /// <summary>
        /// botticelli bot id
        /// </summary>
        [BsonId]
        public string Id { get; set; }
        /// <summary>
        /// A key for a concrete messenger
        /// </summary>
        public string Key { get; set; }
    }
}
