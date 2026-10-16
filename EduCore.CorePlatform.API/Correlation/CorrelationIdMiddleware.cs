using EduCore.CorePlatform.API.Correlation;
using Serilog.Context;
using System.Diagnostics;

namespace EduCore.CorePlatform.API.Correlation;

public class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;

    public CorrelationIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        // 1️ Lấy CorrelationId từ Header hoặc tạo mới
        var correlationId = context.Request.Headers
            .TryGetValue(CorrelationConstants.HeaderName, out var value)
                ? value.ToString()
                : Guid.NewGuid().ToString();

        // 2️ gắn vào Response Header
        context.Response.Headers[CorrelationConstants.HeaderName] = correlationId;

        // 3 Gắn vào HttpContext
        context.Items[CorrelationConstants.LogPropertyName] = correlationId;

        // 4️ TraceId (Activity)
        var traceId = Activity.Current?.TraceId.ToString()
                      ?? context.TraceIdentifier;

        // 5️ Push vào Serilog Context
        using (LogContext.PushProperty(
                   CorrelationConstants.LogPropertyName, correlationId))
        using (LogContext.PushProperty("TraceId", traceId))
        {
            await _next(context);
        }
    }
}
