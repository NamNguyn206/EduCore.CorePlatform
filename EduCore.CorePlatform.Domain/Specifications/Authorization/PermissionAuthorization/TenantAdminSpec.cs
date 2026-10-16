using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Domain.Specifications;
using EduCore.CorePlatform.Domain.Common.Constants.Auth;
using EduCore.CorePlatform.Domain.Entities.MultiTenants.Identities;

namespace EduCore.CorePlatform.Domain.Specifications.Authorization.PermissionAuthorization;

public sealed class TenantAdminSpec : Specification<UserTenantRole>
{
    public TenantAdminSpec(Guid userId, Guid tenantId)
    {
        AndCriteria(x =>
            x.UserTenant.UserId == userId &&
            x.UserTenant.TenantId == tenantId &&
            x.Role.RolePermissions.Any(rp =>
                rp.Permission.Code == PermissionConstants.Tenants.FullAccess));
    }
}
