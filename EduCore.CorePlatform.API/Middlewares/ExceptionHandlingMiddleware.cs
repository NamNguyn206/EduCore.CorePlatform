using EduCore.CorePlatform.Application.Common.Exceptions;
using EduCore.CorePlatform.Domain.Common.Constants.System;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using EduCore.CorePlatform.Application.Abstractions.Localization;

namespace EduCore.CorePlatform.API.Middlewares;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ISystemMessageProvider _systemMessageProvider;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ISystemMessageProvider systemMessageProvider,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _systemMessageProvider = systemMessageProvider;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = exception switch
        {
            BusinessException => HttpStatusCode.BadRequest,
            NotFoundException => HttpStatusCode.NotFound,
            UnauthorizedException => HttpStatusCode.Unauthorized,
            _ => HttpStatusCode.InternalServerError
        };

        var message = exception switch
        {
            BusinessException => exception.Message,
            NotFoundException => await _systemMessageProvider.GetAsync(ErrorCodeConstants.NotFound),
            UnauthorizedException => await _systemMessageProvider.GetAsync(ErrorCodeConstants.Unauthorized),
            _ => await _systemMessageProvider.GetAsync(ErrorCodeConstants.InternalError)
        };

        var response = new
        {
            success = false,
            error = message
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
