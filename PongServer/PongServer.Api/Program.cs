using System.Reflection;
using NLog.Web;
using PongServer.Api.Installers;
using PongServer.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.InstallServicesInAssembly(builder.Configuration, Assembly.GetExecutingAssembly());

// Add NLog
builder.Logging.ClearProviders();
builder.Host.UseNLog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
