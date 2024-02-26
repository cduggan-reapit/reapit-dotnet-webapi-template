using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Reapit.Services.Template.Core;

public static class Startup
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(Startup)));
        services.AddValidatorsFromAssemblyContaining(typeof(Startup));
        
        return services;
    }
}