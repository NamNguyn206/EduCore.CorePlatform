using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Domain.Entities.Identity;
using EduCore.CorePlatform.Infrastructure.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;
using System.Data;
using EduCore.CorePlatform.Domain.Common.Constants.Identities;
using EduCore.CorePlatform.Domain.Common.Enums.Identities;

namespace EduCore.CorePlatform.Infrastructure.Persistence.Seeders.Bootstrap;

public sealed class RoleSeeder : IDataSeeder
{
    public int Order => 1;

    private readonly ApplicationDbContext _db;

    public RoleSeeder(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        // SYSTEM ADMIN ROLE
        if (!await _db.Roles.AnyAsync(x =>
            x.Scope == RoleScope.System &&
            x.Code == RoleConstants.SuperAdmin, cancellationToken))
        {
            _db.Roles.Add(EduCore.CorePlatform.Domain.Entities.Identity.Role.CreateSystemRole(
                RoleConstants.SuperAdmin,
                "Super Admin",
                string.Empty,
                true
            ));
        }

        await _db.SaveChangesAsync(cancellationToken);

        // TENANT ADMIN ROLE
        if (!await _db.Roles.AnyAsync(x => x.Scope == RoleScope.Tenant
                                            && x.Code == RoleConstants.TenantAdmin, cancellationToken))
        {
            _db.Roles.Add(EduCore.CorePlatform.Domain.Entities.Identity.Role.CreateSystemRole(
                RoleConstants.TenantAdmin,
                "Tenant Admin",
                string.Empty,
                true
            ));
        }

        await _db.SaveChangesAsync();

    }
}
