using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Domain.Entities.MultiTenants.Identities;
using EduCore.CorePlatform.Domain.Specifications;

namespace EduCore.CorePlatform.Domain.Specifications.Authorization.PermissionAuthorization;

public sealed class UserPermissionSpec : Specification<UserTenantRole>
{
    public UserPermissionSpec(
        Guid userId,
        Guid tenantId,
        string permissionCode)
    {
        AndCriteria(x =>
            x.UserTenant.UserId == userId &&
            x.UserTenant.TenantId == tenantId &&
            x.Role.RolePermissions.Any(rp =>
                rp.Permission.Code == permissionCode));
    }
}
