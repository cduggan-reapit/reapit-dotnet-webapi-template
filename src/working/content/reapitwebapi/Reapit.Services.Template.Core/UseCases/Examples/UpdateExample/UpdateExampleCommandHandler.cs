using FluentValidation;
using MediatR;
using Reapit.Packages.ErrorHandling.Exceptions;
using Reapit.Services.Template.Core.Helpers;
using Reapit.Services.Template.Domain.Entities.Examples;

namespace Reapit.Services.Template.Core.UseCases.Examples.UpdateExample;

/// <summary>
/// Handler for the <see cref="UpdateExampleCommand"/> request
/// </summary>
public class UpdateExampleCommandHandler : IRequestHandler<UpdateExampleCommand, Example>
{
    private readonly IValidator<UpdateExampleCommand> _validator;

    /// <summary>
    /// Initialize a new instance of <see cref="UpdateExampleCommandHandler"/>
    /// </summary>
    /// <param name="validator">Validator for the <see cref="UpdateExampleCommand"/> class</param>
    public UpdateExampleCommandHandler(IValidator<UpdateExampleCommand> validator)
    {
        _validator = validator;
    }
    
    /// <inheritdoc />
    public async Task<Example> Handle(UpdateExampleCommand request, CancellationToken cancellationToken)
    {
        // Validate the request
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);
        
        // Fetch the entity
        var entity = Example.SeedData.FirstOrDefault(e => e.Id.Equals(request.Id, StringComparison.OrdinalIgnoreCase))
                     ?? throw new NotFoundException(typeof(Example), request.Id);

        // Check the etag
        if (!entity.GetEtag().Equals(request.Etag, StringComparison.OrdinalIgnoreCase))
            throw new ConflictException(typeof(Example), request.Id);
        
        // Apply the update
        entity.Name = request.Name;
        entity.Date = request.Date;
        
        // Commit the changes
        // _ = await _repositoryManager.Examples.UpdateAsync(entity);
        // _ = await _repositoryManager.UnitOfWork.SaveChangesAsync();
        
        // Return the modified entity
        return entity;
    }
}