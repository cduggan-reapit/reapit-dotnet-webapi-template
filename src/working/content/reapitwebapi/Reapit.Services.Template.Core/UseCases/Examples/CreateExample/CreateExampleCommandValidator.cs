using FluentValidation;
using Reapit.Services.Template.Domain.Entities.Examples;

namespace Reapit.Services.Template.Core.UseCases.Examples.CreateExample;

public class CreateExampleCommandValidator : AbstractValidator<CreateExampleCommand>
{
    public CreateExampleCommandValidator()
    {
        RuleFor(cmd => cmd.Name)
            .Must(name => !string.IsNullOrWhiteSpace(name))
            .WithMessage(ValidationMessages.ValueRequired)
            .MaximumLength(Constants.NameMaximumLength)
            .WithMessage(ValidationMessages.ValueExceedsMaximumLengthOf(Constants.NameMaximumLength));

        RuleFor(cmd => cmd.Name)
            .Must(name => !Example.SeedData.Any(e => e.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            .WithMessage(ValidationMessages.ValueMustBeUnique);
    }
}