using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Reapit.Services.Template.Domain.Entities.Examples;

namespace Reapit.Services.Template.Core.UseCases.Examples.PatchExample;

public record PatchExampleCommand(string Id, string Etag, JsonPatchDocument<Example> PatchDocument) : IRequest<Example>;