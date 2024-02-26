using Reapit.Packages.Swagger.Attributes;

namespace Reapit.Services.Template.Api.Models.Examples;

/// <summary>
/// Write model for Examples
/// </summary>
/// <param name="Name">The name of the example</param>
/// <param name="Date">The date of the example</param>
public record ExampleWriteDto(string Name, [field: ShortDate] DateTime Date);