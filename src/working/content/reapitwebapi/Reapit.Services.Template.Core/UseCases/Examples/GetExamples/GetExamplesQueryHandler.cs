using FluentValidation;
using MediatR;
using Reapit.Services.Template.Domain.Entities.Examples;

namespace Reapit.Services.Template.Core.UseCases.Examples.GetExamples;

/// <summary>
/// Handler for the <see cref="GetExamplesQuery"/> request
/// </summary>
public class GetExamplesQueryHandler : IRequestHandler<GetExamplesQuery, IEnumerable<Example>>
{
    private readonly IValidator<GetExamplesQuery> _validator;
    
    /// <summary>
    /// Initialize a new instance of <see cref="GetExamplesQueryHandler"/>
    /// </summary>
    public GetExamplesQueryHandler(IValidator<GetExamplesQuery> validator)
    {
        _validator = validator;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Example>> Handle(GetExamplesQuery request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);
        
        // PageNumber is one-indexed, so decrement to get page offset
        var offset = (request.PageNumber - 1) * request.PageSize;
        return Example.SeedData.Skip(offset).Take(request.PageSize);
    }
}