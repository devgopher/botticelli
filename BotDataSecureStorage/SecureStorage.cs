using BotDataSecureStorage.Entities;
using BotDataSecureStorage.Settings;
using LiteDB;

namespace BotDataSecureStorage
{
    /// <summary>
    /// This storage is intended for keeping bot keys safely
    /// </summary>
    public class SecureStorage 
    {
        private readonly LiteDatabase _db;

        public SecureStorage(SecureStorageSettings settings) 
            => _db = new LiteDatabase(settings.ConnectionString, BsonMapper.Global);

        public BotKey GetBotKey(string botId)
        {
            var allRecs = _db.GetCollection<BotKey>().FindAll().ToList();
            return allRecs.FirstOrDefault(x=> x.Id == botId);
        }

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

        //public void Dispose() => _db?.Dispose();
    }
}