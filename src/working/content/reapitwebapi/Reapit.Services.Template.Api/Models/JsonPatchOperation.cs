using Microsoft.AspNetCore.JsonPatch;

namespace Reapit.Services.Template.Api.Models;

/// <summary>
/// Model representing an entry in a <see cref="JsonPatchDocument{TModel}"/> for example generation
/// </summary>
/// <param name="Op">The value associated with the operation</param>
/// <param name="Path">The path to the property on which to operate</param>
/// <param name="Value">The operation to perform</param>
public record JsonPatchOperation(string Op, string Path, object? Value);