using Botticelli.Chained.Context.Settings;

namespace Botticelli.Chained.Context.InMemory;

public class InMemoryContextStorageBuilder<TKey, TValue> : IContextStorageBuilder<InMemoryStorage<TKey, TValue>, TKey, TValue>
    where TKey : notnull
{
    private int? _capacity;

    public InMemoryStorage<TKey, TValue> Build() =>
        _capacity == null ? new InMemoryStorage<TKey, TValue>() : new InMemoryStorage<TKey, TValue>(_capacity.Value);

    public InMemoryContextStorageBuilder<TKey, TValue> SetInitialCapacity(int capacity)
    {
        _capacity = capacity;

        return this;
    }
}