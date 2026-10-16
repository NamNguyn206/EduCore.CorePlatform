using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Abstractions.Authentication;
using EduCore.CorePlatform.Domain.Entities.Identity;
using EduCore.CorePlatform.Domain.Entities.MultiTenants;
using EduCore.CorePlatform.Infrastructure.Persistence.DbContext;
using EduCore.CorePlatform.Domain.Common.Settings.Seeders;
using EduCore.CorePlatform.Domain.Common.Constants.Identities;
using EduCore.CorePlatform.Domain.Common.Enums.Identities;

namespace EduCore.CorePlatform.Infrastructure.Persistence.Seeders.Bootstrap;

public sealed class BootstrapTenantAdminSeeder : IDataSeeder
{
    public int Order => 6;
    private readonly ApplicationDbContext _db;
    private readonly IPasswordHasher _passwordHasher;
    private readonly BootstrapTenantAdminSettings _bootstrapTenantAdminSettings;
    private readonly ILogger<BootstrapTenantAdminSeeder> _logger;

    public BootstrapTenantAdminSeeder(
        ApplicationDbContext db,
        IPasswordHasher passwordHasher,
        IOptions<BootstrapTenantAdminSettings> settings,
        ILogger<BootstrapTenantAdminSeeder> logger)
    {
        _db = db;
        _bootstrapTenantAdminSettings = settings.Value;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        // 1. Check config flag
        if (!_bootstrapTenantAdminSettings.Enabled)
        {
            _logger.LogInformation("Bootstrap Tenant Admin is disabled.");
            return;
        }

        if (string.IsNullOrWhiteSpace(_bootstrapTenantAdminSettings.Code)
            || string.IsNullOrWhiteSpace(_bootstrapTenantAdminSettings.Email) 
            || string.IsNullOrWhiteSpace(_bootstrapTenantAdminSettings.Password))
        {
            throw new InvalidOperationException("Bootstrap Tenant Admin configuration is missing.");
        }

        // ==========================================================
        // 1️ ENSURE TENANT EXISTS
        // ==========================================================

        Guid? tenantDefaultLanguageId = null;
        if(!string.IsNullOrEmpty(_bootstrapTenantAdminSettings.LanguageCode))
        {
            var tenantDefaultLanguage = await _db.Languages.FirstOrDefaultAsync(x => x.IsDeleted == false
                                                                                    && x.Code == _bootstrapTenantAdminSettings.LanguageCode);

            if(tenantDefaultLanguage != null)
            {
                tenantDefaultLanguageId = tenantDefaultLanguage.Id;
            }    
        }   
        
        var tenant = await _db.Tenants.FirstOrDefaultAsync(x => x.Code == _bootstrapTenantAdminSettings.Code, cancellationToken);

        if (tenant == null)
        {
            tenant = new Tenant(_bootstrapTenantAdminSettings.Code, _bootstrapTenantAdminSettings.Name);
            tenant.Activate();

            _db.Tenants.Add(tenant);            

            _logger.LogInformation("Tenant created: {TenantCode}", _bootstrapTenantAdminSettings.Code);
        }
        else
        {
            tenant.Update(_bootstrapTenantAdminSettings.Name);

            if(!string.IsNullOrEmpty(_bootstrapTenantAdminSettings.LanguageCode))

            _db.Tenants.Update(tenant);
        }    

        if(tenantDefaultLanguageId != null)
        {
            tenant.ChangeDefaultLanguage(tenantDefaultLanguageId.Value, null);
        }    

        await _db.SaveChangesAsync(cancellationToken);

        // ==========================================================
        // 2️ CHECK USER EXISTS
        // ==========================================================
        var existingUser = await _db.Users.FirstOrDefaultAsync(x => x.Email == _bootstrapTenantAdminSettings.Email, cancellationToken);

        if (existingUser != null)
        {
            _logger.LogWarning("Tenant Admin already exists. Skipping bootstrap.");
            return;
        }

        // ==========================================================
        // 3️ CREATE Tenant Admin user
        // ==========================================================
        var passwordHash = _passwordHasher.Hash(_bootstrapTenantAdminSettings.Password);

        var user = new User(            
            _bootstrapTenantAdminSettings.Email,
            passwordHash,
            _bootstrapTenantAdminSettings.FirstName,
            _bootstrapTenantAdminSettings.LastName
        );

        user.Activate();

        _db.Users.Add(user);

        await _db.SaveChangesAsync(cancellationToken);

        // ==========================================================
        // 4️ CREATE USER-TENANT RELATION
        // ==========================================================
        var userTenant = new Domain.Entities.MultiTenants.Identities.UserTenant(
            user.Id,
            tenant.Id,
            isTenantAdmin: true);

        _db.UserTenants.Add(userTenant);
        await _db.SaveChangesAsync(cancellationToken);


        // ==========================================================
        // 5️ GET TENANT ADMIN ROLE
        // ==========================================================
        var tenantAdminRole = await _db.Roles.FirstOrDefaultAsync(x => x.Scope == RoleScope.System
                                                                    && x.Code == RoleConstants.TenantAdmin,
                                                                    cancellationToken);

        if (tenantAdminRole is null)
        {
            throw new InvalidOperationException("Tenant Admin role not found. Run RoleSeeder first.");
        }

        // ==========================================================
        // 6️ ASSIGN ROLE TO USER-TENANT
        // ==========================================================
        var userTenantRole = new Domain.Entities.MultiTenants.Identities.UserTenantRole(
            userTenant.Id,
            tenantAdminRole.Id);

        _db.UserTenantRoles.Add(userTenantRole);

        // ==========================================================
        // 7 UPDATE LANGUAGE (OPTIONAL)
        // ==========================================================
        var language = await _db.Languages.FirstOrDefaultAsync(x => x.Code == _bootstrapTenantAdminSettings.LanguageCode, cancellationToken);
        if (language != null)
            user.ChangeLanguage(language.Id, null);

        await _db.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Tenant Admin bootstrap completed successfully for tenant {TenantCode}", tenant.Code);
    }
}
