using FluentValidation;
using MediatR;
using Reapit.Services.Template.Domain.Entities.Examples;

namespace Reapit.Services.Template.Core.UseCases.Examples.CreateExample;

/// <summary>
/// Handler for the <see cref="CreateExampleCommand"/> request
/// </summary>
public class CreateExampleCommandHandler : IRequestHandler<CreateExampleCommand, Example>
{
    private readonly IValidator<CreateExampleCommand> _validator;

    /// <summary>
    /// Initialize a new instance of <see cref="CreateExampleCommandHandler"/>
    /// </summary>
    /// <param name="validator">Validator for the <see cref="CreateExampleCommand"/> class</param>
    public CreateExampleCommandHandler(IValidator<CreateExampleCommand> validator)
    {
        _validator = validator;
    }
    
    public async Task<Example> Handle(CreateExampleCommand request, CancellationToken cancellationToken)
    {
        // Validate the request
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);
        
        // Apply the update
        var entity = Example.Create(request.Name, request.Date);
        
        // Commit the changes
        // _ = await _repositoryManager.Examples.AddAsync(entity);
        // _ = await _repositoryManager.UnitOfWork.SaveChangesAsync();
        
        // Return the modified entity
        return entity;
    }
}