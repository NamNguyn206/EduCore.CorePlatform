using EduCore.CorePlatform.Application.Common.Exceptions;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Domain.Common.Constants.System;
using EduCore.CorePlatform.Application.Abstractions.Localization;

namespace EduCore.CorePlatform.API.Middlewares;

public sealed class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ISystemMessageProvider _systemMessageProvider;

    public GlobalExceptionHandlerMiddleware(
        RequestDelegate next,
        ISystemMessageProvider systemMessageProvider)
    {
        _next = next;
        _systemMessageProvider = systemMessageProvider;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (BusinessException ex)
        {
            await WriteError(context, ex.ErrorCode, StatusCodes.Status400BadRequest);
        }
        catch (UnauthorizedAccessException)
        {
            await WriteError(context, ErrorCodeConstants.Unauthorized, StatusCodes.Status401Unauthorized);
        }
        catch (Exception)
        {
            await WriteError(context, "SYS_999", StatusCodes.Status500InternalServerError);
        }
    }

    private async Task WriteError(HttpContext ctx, string code, int status)
    {
        ctx.Response.StatusCode = status;
        ctx.Response.ContentType = "application/json";

        var result = new ErrorResult(
            code,
            await _systemMessageProvider.GetAsync(code)
        );

        await ctx.Response.WriteAsJsonAsync(result);
    }
}
