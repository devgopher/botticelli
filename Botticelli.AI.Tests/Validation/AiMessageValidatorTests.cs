using System.Threading.Tasks;
using Botticelli.AI.Message;
using Botticelli.AI.Validation;
using FluentAssertions;
using NUnit.Framework;

namespace Botticelli.AI.Test.Validation;

[TestFixture]
[TestOf(typeof(AiMessageValidator))]
public class AiMessageValidatorTests
{
    private readonly AiMessageValidator _validator = new();

    [Test]
    public async Task ValidateAsync_AllRequiredFieldsFilled_ShouldBeValid()
    {
        var message = CreateValidMessage();

        var result = await _validator.ValidateAsync(message);

        result.IsValid.Should().BeTrue();
    }

    [Test]
    [TestCase("")]
    [TestCase(null)]
    public async Task ValidateAsync_EmptyBody_ShouldBeInvalid(string? body)
    {
        var message = CreateValidMessage();
        message.Body = body;

        var result = await _validator.ValidateAsync(message);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(x => x.PropertyName == nameof(AiMessage.Body));
    }

    [Test]
    [TestCase("")]
    [TestCase(null)]
    public async Task ValidateAsync_EmptyUid_ShouldBeInvalid(string? uid)
    {
        var message = CreateValidMessage();
        message.Uid = uid;

        var result = await _validator.ValidateAsync(message);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(x => x.PropertyName == nameof(AiMessage.Uid));
    }

    [Test]
    [TestCase("")]
    [TestCase(null)]
    public async Task ValidateAsync_EmptyOrNullSubject_ShouldBeInvalid(string? subject)
    {
        var message = CreateValidMessage();
        message.Subject = subject;

        var result = await _validator.ValidateAsync(message);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(x => x.PropertyName == nameof(AiMessage.Subject));
    }

    [Test]
    [TestCase("")]
    [TestCase(null)]
    public async Task ValidateAsync_EmptyOrNullInstruction_ShouldBeInvalid(string? instruction)
    {
        var message = CreateValidMessage();
        message.Instruction = instruction ?? string.Empty;

        var result = await _validator.ValidateAsync(message);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(x => x.PropertyName == nameof(AiMessage.Instruction));
    }

    private static AiMessage CreateValidMessage()
    {
        return new AiMessage
        {
            Uid = "uid-1",
            Subject = "subject",
            Body = "body",
            Instruction = "instruction"
        };
    }
}
