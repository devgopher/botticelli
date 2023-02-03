using BotDataSecureStorage.Entities;
using LiteDB;

namespace BotDataSecureStorage
{
    /// <summary>
    /// This storage is intended for keeping bot keys safely
    /// </summary>
    public class SecureStorage : IDisposable
    {
        private readonly LiteDatabase _db;

        public SecureStorage(string connectionString) => _db = new LiteDatabase(connectionString, BsonMapper.Global);


        public BotKey GetBotKey(string botId) => _db.GetCollection<BotKey>().FindById(botId);
        public void SetBotKey(string botId, string key) => _db.GetCollection<BotKey>().Upsert(botId, new BotKey()
        {
            Id = botId,
            Key = key
        });
        
        public BotData GetBotData(string botId) => _db.GetCollection<BotData>().FindById(botId);
        public void SetBotData(string botId, string[] data) => _db.GetCollection<BotData>().Upsert(botId, new BotData
        {
            Id = botId,
            Data = data
        });




        public void Dispose() => _db.Dispose();
    }
}