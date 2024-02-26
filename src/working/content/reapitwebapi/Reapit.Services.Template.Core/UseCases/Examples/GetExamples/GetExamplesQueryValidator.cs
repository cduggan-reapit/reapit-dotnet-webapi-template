using FluentValidation;

namespace Reapit.Services.Template.Core.UseCases.Examples.GetExamples;

/// <summary>
/// Validator for the <see cref="GetExamplesQuery"/> query
/// </summary>
public class GetExamplesQueryValidator : AbstractValidator<GetExamplesQuery>
{
    /// <summary>
    /// Initialize a new instance of the validator
    /// </summary>
    public GetExamplesQueryValidator()
    {
        RuleFor(request => request.PageNumber)
            .GreaterThan(0)
            .WithMessage(ValidationMessages.ValueMustBeGreaterThanZero);

        RuleFor(request => request.PageSize)
            .GreaterThan(0)
            .WithMessage(ValidationMessages.ValueMustBeGreaterThanZero)
            .LessThanOrEqualTo(QueryConstants.PageSizeLimit)
            .WithMessage(ValidationMessages.ValueExceedsMaximumOf(QueryConstants.PageSizeLimit));
    }
}