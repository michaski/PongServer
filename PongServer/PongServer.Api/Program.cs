using System.Reflection;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using NLog.Web;
using PongServer.Api.BackgroundJobs;
using PongServer.Api.HealthChacks;
using PongServer.Api.Installers;
using PongServer.Api.Middleware;
using PongServer.Application.Dtos.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.InstallServicesInAssembly(builder.Configuration, Assembly.GetExecutingAssembly());

// Add NLog
builder.Logging.ClearProviders();
builder.Host.UseNLog();

var app = builder.Build();

var apiVersionDescriptorProvier = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

app.UseHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = HealthCheckWriter.WriteHealthCheckResponseAsync
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        foreach (var apiVersionDescription in apiVersionDescriptorProvier.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{apiVersionDescription.GroupName}/swagger.json",
                apiVersionDescription.GroupName.ToUpperInvariant());
        }
    });
}

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
