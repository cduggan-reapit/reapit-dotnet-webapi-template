using FluentValidation;
using Reapit.Services.Template.Domain.Entities.Examples;

namespace Reapit.Services.Template.Core.UseCases.Examples.UpdateExample;

public class UpdateExampleCommandValidator : AbstractValidator<UpdateExampleCommand>
{
    public UpdateExampleCommandValidator()
    {
        // Name must be less than a defined length
        RuleFor(cmd => cmd.Name)
            .MaximumLength(Constants.NameMaximumLength)
            .WithMessage(ValidationMessages.ValueExceedsMaximumLengthOf(Constants.NameMaximumLength));

        // Name must be unique to the Id
        RuleFor(cmd => cmd)
            .Must(cmd => !Example.SeedData.Any(e => !e.Id.Equals(cmd.Id, StringComparison.OrdinalIgnoreCase) 
                                                    && e.Name.Equals(cmd.Name, StringComparison.OrdinalIgnoreCase)))
            .WithName(nameof(Example.Name))
            .WithMessage(ValidationMessages.ValueMustBeUnique);
    }
}