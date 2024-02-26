using MediatR;
using Reapit.Packages.ErrorHandling.Exceptions;
using Reapit.Services.Template.Domain.Entities.Examples;

namespace Reapit.Services.Template.Core.UseCases.Examples.GetExampleById;


/// <summary>
/// Handler for the <see cref="GetExampleByIdQuery"/> request
/// </summary>
public class GetExampleByIdQueryHandler : IRequestHandler<GetExampleByIdQuery, Example>
{
    
    /// <summary>
    /// Initialize a new instance of <see cref="GetExampleByIdQueryHandler"/>
    /// </summary>
    public GetExampleByIdQueryHandler()
    {
    }

    /// <inheritdoc/>
    public async Task<Example> Handle(GetExampleByIdQuery request, CancellationToken cancellationToken)
    {
        var example = Example.SeedData.FirstOrDefault(e => e.Id.Equals(request.Id, StringComparison.OrdinalIgnoreCase))
            ?? throw new NotFoundException(typeof(Example), request.Id);

        return await Task.FromResult(example);
    }
}