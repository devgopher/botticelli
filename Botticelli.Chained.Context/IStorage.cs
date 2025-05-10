using System.Diagnostics.CodeAnalysis;

namespace Botticelli.Chained.Context;

/// <summary>
/// Represents a generic key-value storage interface with basic CRUD-like operations.
/// Supports contravariant key types for flexible usage with inheritance hierarchies.
/// </summary>
/// <typeparam name="TKey">The type of keys (contravariant - accepts the specified type or its base types).</typeparam>
/// <typeparam name="TValue">The type of values to store.</typeparam>
public interface IStorage<in TKey, TValue>
    where TKey : notnull
{
    /// <summary>
    /// Determines whether the storage contains the specified key.
    /// </summary>
    /// <param name="key">The key to locate.</param>
    /// <returns>True if the key exists; otherwise, false.</returns>
    bool ContainsKey(TKey key);

    /// <summary>
    /// Adds a new key-value pair to the storage.
    /// </summary>
    /// <param name="key">The key of the element to add.</param>
    /// <param name="value">The value of the element to add.</param>
    /// <exception cref="ArgumentException">Thrown if the key already exists.</exception>
    void Add(TKey key, TValue? value);

    /// <summary>
    /// Removes the key-value pair with the specified key from the storage.
    /// </summary>
    /// <param name="key">The key of the element to remove.</param>
    /// <returns>True if the element was successfully removed; otherwise, false.</returns>
    bool Remove(TKey key);

    /// <summary>
    /// Attempts to retrieve the value associated with the specified key.
    /// </summary>
    /// <param name="key">The key of the value to get.</param>
    /// <param name="value">
    ///     When this method returns, contains the value associated with the specified key,
    ///     if the key is found; otherwise, the default value for the type. 
    ///     This parameter is marked as maybe null when the method returns false.
    /// </param>
    /// <returns>True if the key exists; otherwise, false.</returns>
    bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue? value);
    
    /// <summary>
    /// Gets or sets the value associated with the specified key.
    /// </summary>
    /// <param name="key">The key of the value to get or set.</param>
    /// <returns>The value associated with the specified key.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when getting a key that doesn't exist.</exception>
    /// <exception cref="ArgumentException">Thrown when setting a key that doesn't exist (if the storage requires explicit Add).</exception>
    TValue? this[TKey key] { get; set; }
}