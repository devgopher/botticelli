using Botticelli.Framework.Commands.Utils;
using FluentAssertions;
using NUnit.Framework;
// ReSharper disable InvokeAsExtensionMethod

namespace Botticelli.Framework.Tests.Commands.Utils;

[TestFixture]
public class CommandUtilsTests
{
    [Test]
    public void GetArguments_NullInput_ReturnsEmptyString()
    {
        // Arrange
        string? input = null;

        // Act
        var result = CommandUtils.GetArguments(input);

        // Assert
        result.Should().Be(string.Empty);
    }

    [Test]
    public void GetArguments_EmptyString_ReturnsEmptyString()
    {
        // Arrange
        var input = string.Empty;

        // Act
        var result = CommandUtils.GetArguments(input);

        // Assert
        result.Should().Be(string.Empty);
    }

    [Test]
    public void GetArguments_SingleCommand_ReturnsEmptyString()
    {
        // Arrange
        var input = "/command";

        // Act
        var result = CommandUtils.GetArguments(input);

        // Assert
        result.Should().Be(string.Empty);
    }

    [Test]
    public void GetArguments_CommandWithArguments_ReturnsArguments()
    {
        // Arrange
        var input = "/command argument1 argument2";

        // Act
        var result = CommandUtils.GetArguments(input);

        // Assert
        result.Should().Be("argument1 argument2");
    }

    [Test]
    public void GetArguments_OnlyCommandAndWhitespace_ReturnsEmptyString()
    {
        // Arrange
        var input = "/command     ";

        // Act
        var result = CommandUtils.GetArguments(input);

        // Assert
        result.Should().Be(string.Empty);
    }

    [Test]
    public void GetArguments_CommandWithMixedCase_ReturnsArguments()
    {
        // Arrange
        var input = "/Command Argument1 Argument2";

        // Act
        var result = CommandUtils.GetArguments(input);

        // Assert
        result.Should().Be("Argument1 Argument2");
    }

    [Test]
    public void GetArguments_TrailingSpacesInArguments_ReturnsArgumentsWithoutTrailingSpaces()
    {
        // Arrange
        var input = "/command argument1 argument2     ";

        // Act
        var result = CommandUtils.GetArguments(input);

        // Assert
        result.Should().Be("argument1 argument2");
    }

    [Test]
    public void GetArguments_MultipleSpacesBetweenArguments_ReturnsArguments()
    {
        // Arrange
        var input = "/command   arg1    arg2";

        // Act
        var result = CommandUtils.GetArguments(input);

        // Assert
        result.Should().Be("arg1 arg2");
    }
}
