using Botticelli.Shared.ValueObjects;
using FluentValidation;

namespace Botticelli.Shared.Validation;

// ReSharper disable once UnusedType.Global
public class MessageValidator : AbstractValidator<Message>
{
    public MessageValidator()
    {
        RuleFor(x => x.Body)
                .NotEmpty()
                .When(x => string.IsNullOrWhiteSpace(x.CallbackData))
                .WithMessage("Body is empty: body should be provided when CallbackData is null or empty!");
        
        RuleFor(x => x.CallbackData)
                .NotEmpty()
                .When(x => string.IsNullOrWhiteSpace(x.Body))
                .WithMessage("Callback data is empty: callback data should be provided when Body is null or empty!");;
        
        RuleFor(x => x.Uid).NotEmpty();
    }
}