using MediatR;
using Microsoft.AspNetCore.JsonPatch.Exceptions;
using Reapit.Packages.ErrorHandling.Exceptions;
using Reapit.Services.Template.Core.Helpers;
using Reapit.Services.Template.Domain.Entities.Examples;

namespace Reapit.Services.Template.Core.UseCases.Examples.PatchExample;

/// <summary>
/// Handler for the <see cref="PatchExampleCommand"/> command
/// </summary>
public class PatchExampleCommandHandler : IRequestHandler<PatchExampleCommand, Example>
{
    /// <summary>
    /// Handles the <see cref="PatchExampleCommand"/> request
    /// </summary>
    /// <param name="request">The request</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The updated <see cref="Example"/> object</returns>
    /// <exception cref="NotFoundException">Thrown when the requested entity is not found</exception>
    /// <exception cref="ConflictException">Thrown when the requested entity concurrency check fails</exception>
    /// <exception cref="JsonPatchException">Thrown when the patch document contains an invalid data type (e.g. assigning a non-date value to a date field)</exception>
    public async Task<Example> Handle(PatchExampleCommand request, CancellationToken cancellationToken)
    {
        // Fetch the entity
        var entity = Example.SeedData.FirstOrDefault(e => e.Id.Equals(request.Id, StringComparison.OrdinalIgnoreCase))
                     ?? throw new NotFoundException(typeof(Example), request.Id);

        // Check the etag
        if (!entity.GetEtag().Equals(request.Etag, StringComparison.OrdinalIgnoreCase))
            throw new ConflictException(typeof(Example), request.Id);

        request.PatchDocument.ApplyTo(entity);
        
        // Validate the updated entity
        // var validationResult = await _validator.ValidateAsync(entity, cancellationToken);
        // if (!validationResult.IsValid)
        //    throw new ValidationException(validationResult.Errors);
        
        // Commit the changes
        // _ = await _repositoryManager.Examples.UpdateAsync(entity);
        // _ = await _repositoryManager.UnitOfWork.SaveChangesAsync();

        return await Task.FromResult(entity);
    }
}