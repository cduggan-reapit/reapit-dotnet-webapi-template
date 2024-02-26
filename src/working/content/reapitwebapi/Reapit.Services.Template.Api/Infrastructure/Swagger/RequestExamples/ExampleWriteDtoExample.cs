using Reapit.Services.Template.Api.Models.Examples;
using Swashbuckle.AspNetCore.Filters;

namespace Reapit.Services.Template.Api.Infrastructure.Swagger.RequestExamples;

/// <summary>
/// Swagger example provider for the <see cref="ExampleWriteDto"/> class
/// </summary>
public class ExampleWriteDtoExample : IExamplesProvider<ExampleWriteDto>
{
    /// <inheritdoc/>
    public ExampleWriteDto GetExamples()
        => new("Example #1", new DateTime(2024, 2, 26));
}