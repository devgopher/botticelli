using System;
using System.Collections.Generic;
using System.Text.Json;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using StackExchange.Redis;

namespace Botticelli.Chained.Context.Redis.Tests;

[TestFixture]
public class RedisStorageTests
{
    private Mock<IDatabase> _mockDatabase;
    private RedisStorage<string, string> _redisStorage;
    private Dictionary<string, string> _storage;
    
    [SetUp]
    public void SetUp()
    {
        _mockDatabase = new Mock<IDatabase>();
        var mockConnection = new Mock<IConnectionMultiplexer>();
        mockConnection.Setup(m => m.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Returns(_mockDatabase.Object);
        
        _redisStorage = new RedisStorage<string, string>(_mockDatabase.Object);
        
        // Setup the mock to maintain state
        _mockDatabase.Setup(m => m.StringSet(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), null, When.Always, CommandFlags.None))
                     .Returns<RedisKey, RedisValue, TimeSpan?, When, CommandFlags>((key,
                               value,
                               _,
                               _,
                               _) =>
                     {
                         _storage[key] = value.ToString();
                         return true;
                     });
        
        _mockDatabase.Setup(m => m.StringGet(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
                     .Returns((string key, CommandFlags _) => _storage.TryGetValue(key, out var val) ? val : RedisValue.Null);
        _mockDatabase.Setup(m => m.KeyExists(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
                     .Returns((string key, CommandFlags _) => _storage.ContainsKey(key));
    }

    [Test]
    public void Add_ShouldAddValue_WhenKeyIsNew()
    {
        // Arrange
        var key = "testKey";
        var value = "Test";

        // Act
        Assert.DoesNotThrow(() => _redisStorage.Add(key, value));
    }

    [Test]
    public void Remove_ShouldRemoveValue_WhenKeyExists()
    {
        // Arrange
        var key = "testKey";
        _mockDatabase.Setup(m => m.KeyDelete(key, CommandFlags.None)).Returns(true);

        // Act
        var result = _redisStorage.Remove(key);

        // Assert
        result.Should().BeTrue();
        _mockDatabase.Verify(m => m.KeyDelete(key, CommandFlags.None), Times.Once);
    }

    [Test]
    public void TryGetValue_ShouldReturnTrueAndValue_WhenKeyExists()
    {
        // Arrange
        var key = "testKey";
        var value = "Test";
        _mockDatabase.Setup(m => m.StringGet(key, CommandFlags.None)).Returns(JsonSerializer.Serialize(value));
        _mockDatabase.Setup(m => m.KeyExists(key, CommandFlags.None)).Returns(true);

        // Act
        var result = _redisStorage.TryGetValue(key, out var retrievedValue);

        // Assert
        result.Should().BeTrue();
        retrievedValue.Should().BeEquivalentTo(value);
    }

    [Test]
    public void TryGetValue_ShouldReturnFalse_WhenKeyDoesNotExist()
    {
        // Arrange
        var key = "nonExistentKey";
        _mockDatabase.Setup(m => m.KeyExists(key, CommandFlags.None)).Returns(false);

        // Act
        var result = _redisStorage.TryGetValue(key, out var retrievedValue);

        // Assert
        result.Should().BeFalse();
        retrievedValue.Should().BeNull();
    }
}