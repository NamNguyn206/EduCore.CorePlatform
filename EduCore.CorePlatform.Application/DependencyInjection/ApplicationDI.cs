using FluentValidation;
using EduCore.CorePlatform.Application.Common.Behaviors;
using EduCore.CorePlatform.Application.Common.Services.Security;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace EduCore.CorePlatform.Application.Extensions;

public static class ApplicationDI
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // =========================
        // MediatR
        // =========================
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        // =========================
        // FluentValidation
        // =========================
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // =========================
        // Pipeline Behaviors
        // =========================
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddScoped<IUserAuthorizationService, UserAuthorizationService>();

        return services;
    }
}
