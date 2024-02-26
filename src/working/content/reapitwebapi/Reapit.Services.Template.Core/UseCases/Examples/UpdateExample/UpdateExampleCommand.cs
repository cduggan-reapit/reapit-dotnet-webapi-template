using MediatR;
using Reapit.Services.Template.Domain.Entities.Examples;

namespace Reapit.Services.Template.Core.UseCases.Examples.UpdateExample;

public record UpdateExampleCommand(string Id, string Name, DateTime Date, string Etag) : IRequest<Example>;