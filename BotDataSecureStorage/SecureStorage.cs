﻿using BotDataSecureStorage.Entities;
using BotDataSecureStorage.Settings;
using Botticelli.Shared.ValueObjects;
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

    [Obsolete($"Use {nameof(GetBotContext)}")]
    public BotKey? GetBotKey(string botId)
    {
        using var db = new LiteDatabase(_settings.ConnectionString, BsonMapper.Global);

        return db.GetCollection<BotKey>().FindById(botId);
    }

    [Obsolete($"Use {nameof(SetBotContext)}")]
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

    public BotContext? GetBotContext(string botId)
    {
        using var db = new LiteDatabase(_settings.ConnectionString, BsonMapper.Global);

        return db.GetCollection<BotContext>().FindById(botId);
    }

    public void SetBotContext(BotContext context)
    {
        using var db = new LiteDatabase(_settings.ConnectionString, BsonMapper.Global);

        db.GetCollection<BotContext>()
          .Upsert(context.BotId, context);
    }

    /// <summary>
    ///     Migration to a BotContext from an old-fashioned BotKey
    /// </summary>
    /// <param name="botId"></param>
    public void MigrateToBotContext(string botId)
    {
        #pragma warning disable CS0618
        var botKey = GetBotKey(botId);
        #pragma warning restore CS0618

        if (botKey == null || string.IsNullOrWhiteSpace(botKey.Key)) return;

        using (var db = new LiteDatabase(_settings.ConnectionString, BsonMapper.Global))
        {
            if (db.GetCollection<BotContext>().FindById(botId)?.BotKey != default) return;
        }

        SetBotContext(new BotContext
        {
            BotId = botId,
            BotKey = botKey.Key,
            Items = new Dictionary<string, string>()
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

        return db.GetCollection<T>().FindAll().ToArray();
    }
}