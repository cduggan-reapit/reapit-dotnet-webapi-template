using Reapit.Packages.ErrorHandling.Providers;
using Reapit.Services.Template.Api.Models.Examples;

namespace Reapit.Services.Template.Api.Infrastructure.Swagger.ResponseExamples;

/// <summary>
/// Static class containing classes for the generation of example <see cref="ExampleReadDto"/> objects
/// </summary>
public static class ExampleReadDtoExampleBase
{
    /// <summary>
    /// Get an example <see cref="ExampleReadDto"/> object
    /// </summary>
    public static ExampleReadDto GetExampleModel()
        => new ExampleReadDto(Id: Guid.NewGuid().ToString("D"), 
            "Example #1", 
            new DateTime(2024, 2, 26),
            DateTimeOffsetProvider.Now,
            DateTimeOffsetProvider.Now);
}