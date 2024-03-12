using Microsoft.OpenApi.Models;
using Reapit.Services.Template.Api.Controllers.Abstract;
using Swashbuckle.AspNetCore.Filters;

namespace Reapit.Services.Template.Api.Infrastructure.Swagger;

/// <summary>
/// Extension methods for the configuration of swagger
/// </summary>
public static class SwaggerConfiguration
{
    /// <summary>
    /// Configure services required for swagger
    /// </summary>
    /// <param name="services">The service collection</param>
    public static IServiceCollection AddConfiguredSwagger(this IServiceCollection services)
    {
        services.AddSwaggerExamplesFromAssemblyOf<BaseController>();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(cfg =>
        {
            cfg.SwaggerDoc("v1", new OpenApiInfo { Title = "Template API", Version = "v1" });
            cfg.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Reapit.Services.Template.xml"));
    
            cfg.EnableAnnotations();
            cfg.ExampleFilters();
            cfg.DocumentFilter<JsonPatchDocumentFilter>();
        });

        return services;
    }

    /// <summary>
    /// Register configured swagger middleware
    /// </summary>
    /// <param name="application">The web application used to configure the HTTP request pipeline and routes</param>
    public static WebApplication UseConfiguredSwagger(this WebApplication application)
    {
        if (!application.Environment.IsDevelopment()) 
            return application;
        
        application.UseSwagger();
        application.UseSwaggerUI();

        return application;
    }
}