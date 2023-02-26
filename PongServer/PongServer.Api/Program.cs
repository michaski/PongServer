using System.Reflection;
using PongServer.Api.Installers;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.InstallServicesInAssembly(builder.Configuration, Assembly.GetExecutingAssembly());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
