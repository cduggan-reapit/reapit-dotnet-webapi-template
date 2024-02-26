using MediatR;

namespace Reapit.Services.Template.Core.UseCases.Examples.DeleteExample;

public record DeleteExampleCommand(string Id, string Etag) : IRequest;