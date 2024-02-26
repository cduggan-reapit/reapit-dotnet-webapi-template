using Reapit.Services.Template.Api.Models.Examples;
using Swashbuckle.AspNetCore.Filters;

namespace Reapit.Services.Template.Api.Infrastructure.Swagger.ResponseExamples;

/// <summary>
/// Example provider class for an <see cref="ExampleReadDto"/> model
/// </summary>
public class ExampleReadDtoExample : IExamplesProvider<ExampleReadDto>
{
    /// <inheritdoc/>
    public ExampleReadDto GetExamples()
        => ExampleReadDtoExampleBase.GetExampleModel();
}