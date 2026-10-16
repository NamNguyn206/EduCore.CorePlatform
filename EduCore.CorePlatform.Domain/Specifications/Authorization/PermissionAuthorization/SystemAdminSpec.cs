using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Domain.Specifications;
using EduCore.CorePlatform.Domain.Common.Enums.Identities;
using EduCore.CorePlatform.Domain.Entities.MultiTenants.Identities;

namespace EduCore.CorePlatform.Domain.Specifications.Authorization.PermissionAuthorization;

public sealed class SuperAdminSpec : Specification<UserTenantRole>
{
    public SuperAdminSpec(Guid userId)
    {
        AndCriteria(x =>
            x.UserTenant.UserId == userId &&
            x.Role.Scope == RoleScope.System);
    }
}

