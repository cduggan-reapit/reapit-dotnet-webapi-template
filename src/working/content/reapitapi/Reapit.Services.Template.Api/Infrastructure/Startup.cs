using System.Net.Mime;
using System.Reflection;
using FluentValidation;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Reapit.Packages.Swagger;
using Reapit.Packages.WebHost.Test;

namespace Reapit.Services.Template.Api.Infrastructure;

/// <summary>
/// Startup class responsible for configuring service container and request pipeline
/// </summary>
public class Startup : ITestableStartup
{
    private IWebHostEnvironment Environment { get; }
    public IConfiguration Configuration { get; }
    public IStartupConfigurationService? ExternalStartupConfiguration { get; private set; }
    
    /// <summary>
    /// Initialize a new instance of the <see cref="Startup"/> class
    /// </summary>
    /// <param name="configuration">The configuration service</param>
    /// <param name="environment">The web host environment</param>
    public Startup(IConfiguration configuration, IWebHostEnvironment environment)
    {
        Configuration = configuration;
        Environment = environment;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        // TODO: do we still need/want to do this?
        // It's not possible to inject IStartupConfigurationService
        // but as we only use it for running integration tests we can attempt to pull it out of the service container
        // instead of injecting into the Startup constructor. This MUST happen first otherwise services won't be registered correctly
        try
        {
            var serviceProvider = services.BuildServiceProvider();

            var externalStartupConfiguration = serviceProvider.GetService<IStartupConfigurationService>();

            if (externalStartupConfiguration != null)
            {
                ExternalStartupConfiguration = externalStartupConfiguration;
                ExternalStartupConfiguration.ConfigureEnvironment(Environment);
                ExternalStartupConfiguration.ConfigureService(services, Configuration);
            }
        }
        catch (Exception ex)
        {
            // suppress this exception as IStartupConfigurationService isn't registered for Lambda entry points, but we don't need it for that type of invocation anyway
        }

        services.AddCors(options =>
        {
            string[] exposedHeaders = new string[] { "Location", "If-Match", "ETag" };

            options.AddPolicy(
                "AllowAll",
                builder => builder
                    .AllowAnyMethod()
                    .SetIsOriginAllowed((host) => true)
                    .AllowAnyHeader()
                    .AllowAnyOrigin()
                    .WithExposedHeaders(exposedHeaders));
        });

        services.AddControllers(opts =>
            {
                opts.EnableEndpointRouting = false;
                opts.Filters.Add(new ProducesAttribute(MediaTypeNames.Application.Json));
                opts.Filters.Add(new ConsumesAttribute(MediaTypeNames.Application.Json));
            })
            .ConfigureApiBehaviorOptions(o =>
            {
                o.InvalidModelStateResponseFactory = context =>
                {
                    // TODO replace this with the new error model
                    return new BadRequestObjectResult(""); //new ApiErrorModel(context.ModelState));
                };
            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.NullValueHandling = NullValueHandling.Include;
                options.SerializerSettings.Formatting = Formatting.Indented;
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

                // TODO: add these to the project(?)
                // options.SerializerSettings.ContractResolver = new CamelCaseContractResolver();
                // options.SerializerSettings.Converters.Add(new StringTrimJsonConverter());
            });

        // TODO: change Startup here to something in .Core
        services.AddValidatorsFromAssemblyContaining<Startup>();
        
        services.AddPlatformSwagger<Startup>(options =>
        {
            options.XmlCommentaryPath = $"{nameof(Reapit)}.{nameof(Services)}.{nameof(Template)}" + ".xml";
        });

        // TODO: change this to something from .Core
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Startup>());

        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders =
                ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost;
            options.KnownNetworks.Clear();
            options.KnownProxies.Clear();
        });

        services.AddAutoMapper(cfg => cfg.AddMaps(Assembly.GetExecutingAssembly()));
    }
}