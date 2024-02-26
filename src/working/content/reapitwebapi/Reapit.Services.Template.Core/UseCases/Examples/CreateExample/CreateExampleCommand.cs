using MediatR;
using Reapit.Services.Template.Domain.Entities.Examples;

namespace Reapit.Services.Template.Core.UseCases.Examples.CreateExample;

public record CreateExampleCommand(string Name, DateTime Date) : IRequest<Example>;