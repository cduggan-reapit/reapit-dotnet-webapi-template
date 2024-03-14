using Microsoft.OpenApi.Models;
using Reapit.Packages.Extensions.Object;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Reapit.Services.Template.Api.Infrastructure.Swagger;

public class JsonPatchDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        // Remove irrelevant schema
        var schemas = swaggerDoc.Components.Schemas.ToList();
        foreach (var schema in schemas.Where(schema => schema.Key.StartsWith("Operation")))
            swaggerDoc.Components.Schemas.Remove(schema.Key);
        
        // Add a new schema for PatchDocuments
        swaggerDoc.Components.Schemas.Add("Operation", new OpenApiSchema
        {
            Type = "object",
            Properties = new Dictionary<string, OpenApiSchema>
            {
                {"op", new OpenApiSchema{ Type = "string" } },
                {"value", new OpenApiSchema{ Type = "object", Nullable = true } },
                {"path", new OpenApiSchema{ Type = "string" } }
            }
        });

        swaggerDoc.Components.Schemas.Add("JsonPatchDocument", new OpenApiSchema
        {
            Type = "array",
            Items = new OpenApiSchema
            {
                Reference = new OpenApiReference { Type = ReferenceType.Schema, Id = "Operation" }
            },
            Description = "Array of operations to perform"
        });
        
        // Fix the references
        foreach(var path in swaggerDoc.Paths.SelectMany(p => p.Value.Operations)
                    .Where(p => p.Key == OperationType.Patch))
        {
            foreach (var item in path.Value.RequestBody.Content.Where(c => c.Key != "application/json-patch+json"))
                path.Value.RequestBody.Content.Remove(item.Key);
            
            var response = path.Value.RequestBody.Content.Single(c => c.Key == "application/json-patch+json");
            response.Value.Schema = new OpenApiSchema
            {
                Reference = new OpenApiReference { Type = ReferenceType.Schema, Id = "JsonPatchDocument" }
            };
        }
    }
}