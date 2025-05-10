namespace Botticelli.Framework.Chained.Context.Settings;

public interface IContextStorageBuilder<TStorage, TKey, TValue>
where TStorage : IStorage<TKey, TValue> 
where TKey : notnull
{
    private TStorage Storage => throw new NotImplementedException();
    
    public TStorage Build();
}