using Reapit.Services.Template.Api.Models;
using Reapit.Services.Template.Api.Models.Examples;
using Swashbuckle.AspNetCore.Filters;

namespace Reapit.Services.Template.Api.Infrastructure.Swagger.RequestExamples;

/// <summary>
/// Swagger example provider for patches to the <see cref="ExampleWriteDto"/> class
/// </summary>
public class ExampleWriteDtoPatchExample : IExamplesProvider<JsonPatchOperation[]>
{
    /// <inheritdoc/>
    public JsonPatchOperation[] GetExamples()
        => [
            new JsonPatchOperation(Op: "replace", Path: "/name", Value: "Patched Example"),
            new JsonPatchOperation(Op: "replace", Path: "/date", Value: "2023-02-26")
        ];
}