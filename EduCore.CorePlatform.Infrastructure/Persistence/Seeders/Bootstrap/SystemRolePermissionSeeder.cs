using EduCore.CorePlatform.Domain.Common.Constants.Identities;
using EduCore.CorePlatform.Domain.Common.Enums.Identities;
using EduCore.CorePlatform.Domain.Entities.Identity;
using EduCore.CorePlatform.Infrastructure.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Infrastructure.Persistence.Seeders.Bootstrap;

public sealed class SystemRolePermissionSeeder : IDataSeeder
{
    public int Order => 3;

    private readonly ApplicationDbContext _db;

    public SystemRolePermissionSeeder(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task SeedAsync(CancellationToken ct = default)
    {
        var systemAdminRole = await _db.Roles.FirstOrDefaultAsync(
            x => x.Scope == RoleScope.System
              && x.Code == RoleConstants.SuperAdmin,
            ct);

        if (systemAdminRole is null)
            throw new InvalidOperationException(
                "SuperAdmin role not found. Run RoleSeeder first.");

        var alreadySeeded = await _db.RolePermissions
            .AnyAsync(x => x.RoleId == systemAdminRole.Id, ct);

        if (alreadySeeded)
            return;

        var permissions = await _db.Permissions.ToListAsync(ct);

        foreach (var permission in permissions)
        {
            _db.RolePermissions.Add(new RolePermission(systemAdminRole.Id, permission.Id, null));
        }

        await _db.SaveChangesAsync(ct);
    }
}
