using CouchDB.Driver.Exceptions;
using System.Text.Json;

namespace API.Middleware
{
    public class ConfigurationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ConfigurationMiddleware> _logger;

        public ConfigurationMiddleware(RequestDelegate next, ILogger<ConfigurationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON serialization error");
                await HandleExceptionAsync(context, ex, StatusCodes.Status400BadRequest, "Invalid json format");
            }
            catch (IOException ex)
            {
                _logger.LogError(ex, "File system error");
                await HandleExceptionAsync(context, ex, StatusCodes.Status500InternalServerError, "Configuration storage error");
            }
            catch (CouchNotFoundException ex)
            {
                _logger.LogWarning(ex, "CocuhDB document not found");
                await HandleExceptionAsync(context, ex, StatusCodes.Status404NotFound, "Document not found");
            }
            catch (CouchConflictException ex)
            {
                _logger.LogWarning(ex, "CocuhDB document conflict");
                await HandleExceptionAsync(context, ex, StatusCodes.Status409Conflict, "Document version conflict");
            }
            catch (CouchException ex)
            {
                _logger.LogError(ex, "CouchDB error");
                await HandleExceptionAsync(context, ex, StatusCodes.Status500InternalServerError, "Database error");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred");
                await HandleExceptionAsync(context, ex, StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception, int statusCode, string message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            await context.Response.WriteAsJsonAsync(new
            {
                StatusCode = statusCode,
                Message = message
            });
        }

    }
}
