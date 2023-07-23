using BotDataSecureStorage.Entities;
using BotDataSecureStorage.Settings;
using LiteDB;

namespace BotDataSecureStorage;

/// <summary>
///     This storage is intended for keeping bot keys safely
/// </summary>
public class SecureStorage
{
    private readonly SecureStorageSettings _settings;

    public SecureStorage(SecureStorageSettings settings)
        => _settings = settings;

    public BotKey? GetBotKey(string botId)
    {
        using var db = new LiteDatabase(_settings.ConnectionString, BsonMapper.Global);

        return db.GetCollection<BotKey>().FindById(botId);
    }

    public void SetBotKey(string botId, string key)
    {
        using var db = new LiteDatabase(_settings.ConnectionString, BsonMapper.Global);

        db.GetCollection<BotKey>()
          .Upsert(botId,
                  new BotKey
                  {
                      Id = botId,
                      Key = key
                  });
    }

    public BotData? GetBotData(string botId)
    {
        using var db = new LiteDatabase(_settings.ConnectionString, BsonMapper.Global);

        var allRecs = db.GetCollection<BotData>().FindAll();

        return allRecs.FirstOrDefault(x => x.Id == botId);
    }

    public void SetBotData(string botId, string[] data)
    {
        using var db = new LiteDatabase(_settings.ConnectionString, BsonMapper.Global);

        db.GetCollection<BotData>()
          .Upsert(botId,
                  new BotData
                  {
                      Id = botId,
                      Data = data
                  });
    }

    public void SetAnyData<T>(string id, T data)
            where T : IDbEntity
    {
        using var db = new LiteDatabase(_settings.ConnectionString, BsonMapper.Global);

        db.GetCollection<T>()
          .Upsert(id, data);
    }

    public T GetAnyData<T>(string id)
            where T : IDbEntity 
        => GetAnyData<T>().FirstOrDefault(x => x.Id == id);

    public IEnumerable<T> GetAnyData<T>()
            where T : IDbEntity
    {
        using var db = new LiteDatabase(_settings.ConnectionString, BsonMapper.Global);

        return db.GetCollection<T>().FindAll();
    }
}