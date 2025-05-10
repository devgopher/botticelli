using System.Diagnostics.CodeAnalysis;

namespace Botticelli.Framework.Chained.Context.InMemory;

/// <summary>
///     In-memory storage
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
public class InMemoryStorage<TKey, TValue> : IStorage<TKey, TValue>
    where TKey : notnull
{
    private readonly Dictionary<TKey, TValue?> _dictionary;

    public InMemoryStorage()
    {
        _dictionary = new Dictionary<TKey, TValue?>();
    }

    public InMemoryStorage(int capacity)
    {
        _dictionary = new Dictionary<TKey, TValue?>(capacity);
    }

    /// <inheritdoc />
    public bool ContainsKey(TKey key) => _dictionary.ContainsKey(key);

    /// <inheritdoc />
    public void Add(TKey key, TValue? value) => _dictionary.Add(key, value);

    /// <inheritdoc />
    public bool Remove(TKey key) => _dictionary.Remove(key);

    /// <inheritdoc />
    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value) =>
        _dictionary.TryGetValue(key, out value);

    /// <inheritdoc />
    public TValue? this[TKey key]
    {
        get => _dictionary[key];
        set => _dictionary[key] = value;
    }
}