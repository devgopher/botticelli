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
        public string Id { get; set; }
        /// <summary>
        /// A key for a concrete messenger
        /// </summary>
        public string Key { get; set; }
    }
}
