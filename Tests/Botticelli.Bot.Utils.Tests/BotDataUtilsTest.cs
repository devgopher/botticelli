using System.IO;
using FluentAssertions;
using NUnit.Framework;

namespace Botticelli.Bot.Utils.Tests;

[TestFixture]
public class BotDataUtilsTests
{
    [SetUp]
    public void Setup()
    {
        // Ensure the test directory is clean before each test
        if (Directory.Exists(DataDirectory)) Directory.Delete(DataDirectory, true);
    }

    [TearDown]
    public void TearDown()
    {
        // Clean up the test directory after each test
        if (Directory.Exists(DataDirectory)) Directory.Delete(DataDirectory, true);
    }

    private const string DataDirectory = "Data";

    [Test]
    public void GetBotId_Should_CreateFile_And_ReturnNewBotId_When_FileDoesNotExist()
    {
        // Ensure the main directory does not exist before the test
        if (Directory.Exists(DataDirectory)) Directory.Delete(DataDirectory, true);
        
        // Act
        var botId = BotDataUtils.GetBotId();

        // Assert
        botId.Should().NotBeNullOrWhiteSpace();
    }

    [Test]
    public void GetBotId_Should_CreateDirectory_When_SubDirDoesNotExist()
    {
        // Ensure the main directory does not exist before the test
        if (Directory.Exists(DataDirectory)) Directory.Delete(DataDirectory, true);

        // Act
        BotDataUtils.GetBotId();

        // Assert
        Directory.Exists(DataDirectory).Should().BeTrue();
    }
}