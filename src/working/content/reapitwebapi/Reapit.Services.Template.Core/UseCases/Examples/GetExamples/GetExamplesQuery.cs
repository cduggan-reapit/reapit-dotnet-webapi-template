using MediatR;
using Reapit.Services.Template.Domain.Entities.Examples;

namespace Reapit.Services.Template.Core.UseCases.Examples.GetExamples;

/// <summary>
/// 
/// </summary>
/// <param name="PageNumber"></param>
/// <param name="PageSize"></param>
public record GetExamplesQuery(int PageNumber, int PageSize) : IRequest<IEnumerable<Example>>;