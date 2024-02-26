using MediatR;
using Reapit.Packages.ErrorHandling.Exceptions;
using Reapit.Services.Template.Core.Helpers;
using Reapit.Services.Template.Core.UseCases.Examples.UpdateExample;
using Reapit.Services.Template.Domain.Entities.Examples;

namespace Reapit.Services.Template.Core.UseCases.Examples.DeleteExample;

/// <summary>
/// Handler for the <see cref="UpdateExampleCommand"/> request
/// </summary>
public class DeleteExampleCommandHandler : IRequestHandler<DeleteExampleCommand>
{
    /// <summary>
    /// Initialize a new instance of <see cref="DeleteExampleCommandHandler"/>
    /// </summary>
    public DeleteExampleCommandHandler()
    {
    }
    
    /// <inheritdoc />
    public Task Handle(DeleteExampleCommand request, CancellationToken cancellationToken)
    {
        // Fetch the entity
        var entity = Example.SeedData.FirstOrDefault(e => e.Id.Equals(request.Id, StringComparison.OrdinalIgnoreCase))
                     ?? throw new NotFoundException(typeof(Example), request.Id);

        // Check the etag
        if (!entity.GetEtag().Equals(request.Etag, StringComparison.OrdinalIgnoreCase))
            throw new ConflictException(typeof(Example), request.Id);
        
        // Commit the changes
        // _ = await _repositoryManager.Examples.RemoveAsync(entity);
        // _ = await _repositoryManager.UnitOfWork.SaveChangesAsync();

        return Task.CompletedTask;
    }
}