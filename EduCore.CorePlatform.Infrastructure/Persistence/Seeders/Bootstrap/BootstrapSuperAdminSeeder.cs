using EduCore.CorePlatform.Application.Abstractions.Authentication;
using EduCore.CorePlatform.Domain.Entities.Identity;
using EduCore.CorePlatform.Infrastructure.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Domain.Common.Settings.Seeders;
using EduCore.CorePlatform.Domain.Common.Constants.Identities;
using EduCore.CorePlatform.Domain.Common.Enums.Identities;

namespace EduCore.CorePlatform.Infrastructure.Persistence.Seeders.Bootstrap;

public sealed class BootstrapSuperAdminSeeder: IDataSeeder
{
    public int Order => 5;
    private readonly ApplicationDbContext _db;
    private readonly IPasswordHasher _passwordHasher;
    private readonly BootstrapSuperAdminSettings _bootstrapSuperAdminSettings;
    private readonly ILogger<BootstrapSuperAdminSeeder> _logger;

    public BootstrapSuperAdminSeeder(
        ApplicationDbContext db,
        IPasswordHasher passwordHasher,
        IOptions<BootstrapSuperAdminSettings> bootstrapSuperAdminSettings,
        ILogger<BootstrapSuperAdminSeeder> logger)
    {
        _db = db;
        _bootstrapSuperAdminSettings = bootstrapSuperAdminSettings.Value;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        // 1. Check config flag
        if (!_bootstrapSuperAdminSettings.Enabled)
        {
            _logger.LogInformation("Bootstrap System Admin is disabled.");
            return;
        }

        // 1️ Check existing System Admin
        var exists = await _db.Users.AnyAsync(x => x.Email == _bootstrapSuperAdminSettings.Email, cancellationToken);

        if (exists)
        {
            _logger.LogWarning("System Admin already exists. Skipping bootstrap.");
            return;
        }

        if (string.IsNullOrWhiteSpace(_bootstrapSuperAdminSettings.Email) || string.IsNullOrWhiteSpace(_bootstrapSuperAdminSettings.Password))
            throw new InvalidOperationException("Bootstrap SuperAdmin config is missing");

        // 4. HASH PASSWORD (QUAN TRỌNG)
        var passwordHash = _passwordHasher.Hash(_bootstrapSuperAdminSettings.Password);

        // 2️ Get SuperAdmin role
        var systemAdminRole = await _db.Roles.FirstOrDefaultAsync(x => x.Scope == RoleScope.System
                                                                    && x.Code == RoleConstants.SuperAdmin,
                                                                    cancellationToken);

        if (systemAdminRole is null)
        {
            throw new InvalidOperationException("SuperAdmin role not found. Run RoleSeeder first.");
        }

        // 3️ Create user
        var user = User.CreateSuperAdmin(
            _bootstrapSuperAdminSettings.Email,
            passwordHash);

        // ==========================================================
        // UPDATE LANGUAGE (OPTIONAL)
        // ==========================================================
        var language = await _db.Languages.FirstOrDefaultAsync(x => x.Code == _bootstrapSuperAdminSettings.LanguageCode, cancellationToken);
        if(language != null)
            user.ChangeLanguage(language.Id, null);

        _db.Users.Add(user);

        await _db.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("System Admin bootstrap completed successfully.");
    }
}
