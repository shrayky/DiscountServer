namespace API.Middleware
{
    public static class ConfigurationMiddlewareExtensions
    {
        public static IApplicationBuilder UseConfigurationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ConfigurationMiddleware>();
        }
    }
}
