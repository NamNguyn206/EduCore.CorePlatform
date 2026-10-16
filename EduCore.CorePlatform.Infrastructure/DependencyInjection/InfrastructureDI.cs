using EduCore.CorePlatform.Application.Abstractions.Authentication;
using EduCore.CorePlatform.Application.Abstractions.Events;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Domain.Common.Settings.Auth;
using EduCore.CorePlatform.Domain.Common.Settings.Seeders;
using EduCore.CorePlatform.Domain.Common.Settings.System;
using EduCore.CorePlatform.Infrastructure.Events;
using EduCore.CorePlatform.Infrastructure.Persistence.DbContext;
using EduCore.CorePlatform.Infrastructure.Persistence.Repositories;
using EduCore.CorePlatform.Infrastructure.Persistence.Seeders;
using EduCore.CorePlatform.Infrastructure.Persistence.Seeders.Bootstrap;
using EduCore.CorePlatform.Infrastructure.Persistence.Seeders.Bootstrap.Permission;
using EduCore.CorePlatform.Infrastructure.Persistence.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EduCore.CorePlatform.Application.Abstractions.Persistence;
using EduCore.CorePlatform.Application.Abstractions.Storage;
using EduCore.CorePlatform.Application.Abstractions.Localization;
using EduCore.CorePlatform.Infrastructure.Authentication;
using EduCore.CorePlatform.Infrastructure.Authorization;
using EduCore.CorePlatform.Infrastructure.Authorization.Scanning;
using EduCore.CorePlatform.Infrastructure.MultiTenants;
using EduCore.CorePlatform.Infrastructure.Localization;
using EduCore.CorePlatform.Infrastructure.Messaging;
using EduCore.CorePlatform.Infrastructure.Storages;

namespace EduCore.CorePlatform.Infrastructure.DependencyInjection;

public static class InfrastructureDI
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // ============================
        // Settings
        // ============================
        services.Configure<BootstrapSuperAdminSettings>(configuration.GetSection("BootstrapSuperAdmin"));
        services.Configure<BootstrapTenantAdminSettings>(configuration.GetSection("BootstrapTenantAdmin"));
        services.Configure<ApplicationSettings>(configuration.GetSection("ApplicationSettings"));
        services.Configure<FileStorageSettings>(configuration.GetSection("ApplicationSettings:FileStorage"));
        services.Configure<JwtSettings>(configuration.GetSection("ApplicationSettings:Jwt"));
        services.Configure<DataTypeFormatSettings>(configuration.GetSection("ApplicationSettings:DataTypeFormat"));
        

        services.AddHttpContextAccessor();

        // ============================
        // DbContext
        // ============================
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });

        // ============================
        // Localization
        // ============================
        services.AddScoped<ISystemMessageProvider, SystemMessageProvider>();
        services.AddScoped<ICurrentLanguage, CurrentLanguage>();

        // ============================
        // Authorization
        // ============================
        services.AddScoped<ICurrentUser, CurrentUser>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
        services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
        

        // ============================
        // Authentication
        // ============================
        services.AddSingleton<IPasswordHasher, AspNetPasswordHasher>();
        services.AddScoped<ITokenService, TokenService>();

        // ============================
        // MultiTenants
        // ============================
        services.AddScoped<ICurrentTenant, CurrentTenant>();

        // ============================
        // Persistence
        // ============================
        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IDomainEventDispatcher, MediatRDomainEventDispatcher>();

        // ============================
        // Storage
        // ============================
        services.AddScoped<IFileStorageService, FileStorageService>();

        // ============================
        // Seeders
        // ============================
        // Seeder runner
        services.AddScoped<SeederRunner>();

        // Individual seeders        
        services.AddScoped<IDataSeeder, PermissionSeeder>();
        services.AddScoped<IPermissionDiscoveryService, PermissionDiscoveryService>();
        services.AddScoped<IDataSeeder, BootstrapSuperAdminSeeder>();
        services.AddScoped<IDataSeeder, BootstrapTenantAdminSeeder>();
        services.AddScoped<IDataSeeder, LanguageSeeder>();
        services.AddScoped<IDataSeeder, RoleSeeder>();
        services.AddScoped<IDataSeeder, SystemRolePermissionSeeder>();

        return services;
    }
}
