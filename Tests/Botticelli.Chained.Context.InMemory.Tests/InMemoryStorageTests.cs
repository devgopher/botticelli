using FluentAssertions;

namespace Botticelli.Chained.Context.InMemory.Tests;

[TestFixture]
public class InMemoryStorageTests
{
    [SetUp]
    public void SetUp()
    {
        _storage = new InMemoryStorage<string, int>();
    }

    private InMemoryStorage<string, int> _storage;

    [Test]
    [TestCase("testKey", 128923)]
    public void Add_ShouldAddValue_WhenKeyIsNew(string key, int value)
    {
        // Act
        _storage.Add(key, value);

        // Assert
        _storage.ContainsKey(key).Should().BeTrue();
        _storage[key].Should().Be(value);
    }

    [Test]
    [TestCase("testKey", 1343223)]
    public void Remove_ShouldRemoveValue_WhenKeyExists(string key, int value)
    {
        // Arrange
        _storage.Add(key, value);

        // Act
        var result = _storage.Remove(key);

        // Assert
        result.Should().BeTrue();
        _storage.ContainsKey(key).Should().BeFalse();
    }

    [Test]
    [TestCase("testKey", 33432)]
    public void TryGetValue_ShouldReturnTrueAndValue_WhenKeyExists(string key, int value)
    {
        // Arrange
        _storage.Add(key, value);

        // Act
        var result = _storage.TryGetValue(key, out var retrievedValue);

        // Assert
        result.Should().BeTrue();
        retrievedValue.Should().Be(value);
    }

    [Test]
    public void TryGetValue_ShouldReturnFalse_WhenKeyDoesNotExist()
    {
        // Arrange
        var key = "nonExistentKey";

        // Act
        var result = _storage.TryGetValue(key, out var retrievedValue);

        // Assert
        result.Should().BeFalse();
        retrievedValue.Should().Be(default);
    }

    [Test]
    [TestCase("testKey", 33432)]
    public void Indexer_ShouldGetAndSetValue(string key, int value)
    {
        // Act
        _storage[key] = value;

        // Assert
        _storage[key].Should().Be(value);
    }
}