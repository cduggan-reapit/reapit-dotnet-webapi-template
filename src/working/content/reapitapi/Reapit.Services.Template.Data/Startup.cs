using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reapit.Services.Template.Data.Dynamo;

namespace Reapit.Services.Template.Data;

/// <summary>
/// Extension methods for the configuration of data services
/// </summary>
public static class Startup
{
    /// <summary>
    /// Configure services required for the data layer
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="configuration">The applications configuration properties</param>
    public static IServiceCollection AddDataServices(IServiceCollection services, IConfiguration configuration)
    {
        #if (isDynamoDataLayer)
        // Register IOptions<DynamoDbConfiguration>
        services.Configure<DynamoDbConfiguration>(configuration.GetSection(key: DynamoDbConfiguration.Key));
        
        // Register DynamoDbServices
        
        #elif (isMysqlDataLayer)
        // Register DbContext & DbContextFactory
        #endif
        
        return services;
    }
}
