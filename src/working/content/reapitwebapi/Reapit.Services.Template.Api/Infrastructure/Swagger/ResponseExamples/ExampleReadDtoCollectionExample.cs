using Reapit.Packages.Paging;
using Reapit.Services.Template.Api.Models.Examples;
using Swashbuckle.AspNetCore.Filters;

namespace Reapit.Services.Template.Api.Infrastructure.Swagger.ResponseExamples;

/// <summary>
/// Example provider class for an <see cref="PagedResult{ExampleReadDto}"/> model
/// </summary>
public class ExampleReadDtoCollectionExample : IExamplesProvider<IEnumerable<ExampleReadDto>>
{
    /// <inheritdoc />
    public IEnumerable<ExampleReadDto> GetExamples()
        => new[] { ExampleReadDtoExampleBase.GetExampleModel() };
}