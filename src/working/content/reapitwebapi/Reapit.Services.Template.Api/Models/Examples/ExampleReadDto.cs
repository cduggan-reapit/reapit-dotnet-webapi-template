using Reapit.Packages.Swagger.Attributes;

namespace Reapit.Services.Template.Api.Models.Examples;

/// <summary>
/// Read model for Examples
/// </summary>
/// <param name="Id">Unique identifier of the example</param>
/// <param name="Name">The name of the example</param>
/// <param name="Date">The date of the example</param>
/// <param name="DateCreated">Timestamp of the examples creation</param>
/// <param name="DateLastModified">Timestamp of the last modification to the example</param>
public record ExampleReadDto(string Id, string Name, [property: ShortDate] DateTime Date, DateTimeOffset DateCreated, DateTimeOffset DateLastModified);