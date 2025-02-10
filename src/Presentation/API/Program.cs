using Application.Interfaces;
using API.Middleware;
using Shared.Configuration.interfaces;
using Shared.Logging;
using System.Text;
using API.Extensions;
using Scalar.AspNetCore;
using Configuration;
using MemCache;

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

builder.Services.AddSingleton<ICacheService, MemoryCacheService>();
builder.Services.AddSingleton<IConfigurationService, ConfigurationService>();

builder.ApplyAppConfigurationExtension();

builder.Services.AddCustomLogging();

builder.Services.AddWindowsService();

var app = builder.Build();

app.UseSwagger(options =>
{
    options.RouteTemplate = "/openapi/{documentName}.json";
});
app.MapScalarApiReference();

app.UseAuthorization();

app.UseConfigurationMiddleware();

app.MapControllers();

app.Run();
