using System.Reflection;
using Microsoft.OpenApi.Models;
using Reapit.Packages.ErrorHandling;
using Reapit.Services.Template.Api.Controllers.Abstract;
using Reapit.Services.Template.Api.Infrastructure.Swagger;
using Reapit.Services.Template.Core;
using Swashbuckle.AspNetCore.Filters;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddCoreServices();

// Use controllers
builder.Services.AddControllers()
    .AddNewtonsoftJson();
builder.Services.RegisterErrorHandlerServices();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerExamplesFromAssemblyOf<BaseController>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(cfg =>
{
    cfg.SwaggerDoc("v1", new OpenApiInfo { Title = "Template API", Version = "v1" });
    cfg.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Reapit.Services.Template.xml"));
    
    cfg.EnableAnnotations();
    cfg.ExampleFilters();
    cfg.DocumentFilter<JsonPatchDocumentFilter>();
});


var app = builder.Build();

app.UseExceptionHandler(_ => { });

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();
app.UseRouting();


app.Run();
