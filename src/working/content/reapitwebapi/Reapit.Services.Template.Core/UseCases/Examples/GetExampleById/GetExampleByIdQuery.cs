using MediatR;
using Reapit.Services.Template.Domain.Entities.Examples;

namespace Reapit.Services.Template.Core.UseCases.Examples.GetExampleById;

public record GetExampleByIdQuery(string Id) : IRequest<Example>;