using System.Reflection;
using Reapit.Packages.ErrorHandling;
using Reapit.Services.Template.Api.Infrastructure.Swagger;
using Reapit.Services.Template.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddCoreServices();
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.RegisterErrorHandlerServices();
builder.Services.AddConfiguredSwagger();

var app = builder.Build();

app.UseExceptionHandler(_ => { });
app.UseConfiguredSwagger();
app.UseHttpsRedirection();
app.MapControllers();
app.UseRouting();

app.Run();
