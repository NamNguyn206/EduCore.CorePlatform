using Serilog.Context;

namespace EduCore.CorePlatform.API.Middlewares;
public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(
        RequestDelegate next,
        ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        using (LogContext.PushProperty("RequestPath", context.Request.Path))
        using (LogContext.PushProperty("Method", context.Request.Method))
        {
            _logger.LogInformation("Incoming request");

            try
            {
                await _next(context);
                _logger.LogInformation(
                    "Request completed with status {StatusCode}",
                    context.Response.StatusCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");
                throw;
            }
        }
    }
}
