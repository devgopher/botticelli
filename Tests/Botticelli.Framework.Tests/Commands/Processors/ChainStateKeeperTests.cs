using System.Collections.Generic;
using Botticelli.Framework.Commands.Processors;
using FluentAssertions;
using NUnit.Framework;

namespace Botticelli.Framework.Tests.Commands.Processors
{
    [TestFixture]
    public class ChainStateKeeperTests
    {
        private const string ChatId1 = "chat1";
        private const string ChatId2 = "chat2";
        
        [SetUp]
        public void Setup()
        {
            // Reset the state before each test
            ChainStateKeeper.SetState(ChatId1, false);
            ChainStateKeeper.SetState(ChatId2, false);
        }

        [Test]
        public void SetState_Should_SetSingleChatIdState()
        {
            // Act
            ChainStateKeeper.SetState(ChatId1, true);

            // Assert
            ChainStateKeeper.GetState(ChatId1).Should().BeTrue();
            ChainStateKeeper.GetState(ChatId2).Should().BeFalse();
        }

        [Test]
        public void GetState_Should_ReturnFalse_For_UnsetChatId()
        {
            // Act & Assert
            ChainStateKeeper.GetState("unknownChatId").Should().BeFalse();
        }

        [Test]
        public void SetState_Should_SetMultipleChatIdsState()
        {
            // Act
            ChainStateKeeper.SetState(new List<string> { ChatId1, ChatId2 }, true);

            // Assert
            ChainStateKeeper.GetState(ChatId1).Should().BeTrue();
            ChainStateKeeper.GetState(ChatId2).Should().BeTrue();
        }

        [Test]
        public void SetState_Should_SetMultipleChatIds_To_False()
        {
            // Arrange
            ChainStateKeeper.SetState(ChatId1, true);
            ChainStateKeeper.SetState(ChatId2, true);

            // Act
            ChainStateKeeper.SetState(new List<string> { ChatId1, ChatId2 }, false);

            // Assert
            ChainStateKeeper.GetState(ChatId1).Should().BeFalse();
            ChainStateKeeper.GetState(ChatId2).Should().BeFalse();
        }

        [Test]
        public void SetState_Should_Override_PreviousState()
        {
            // Arrange
            ChainStateKeeper.SetState(ChatId1, true);

            // Act
            ChainStateKeeper.SetState(ChatId1, false);

            // Assert
            ChainStateKeeper.GetState(ChatId1).Should().BeFalse();
        }
    }
}
