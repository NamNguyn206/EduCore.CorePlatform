using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Domain.Specifications;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Domain.Entities.MultiTenants.Identities;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.TenantUsers.Specifications.Commands;

public sealed class TenantUserByUserIdAndTenantIdForDeleteSpec
    : Specification<UserTenant>
{
    public TenantUserByUserIdAndTenantIdForDeleteSpec(ICurrentTenant currentTenant, Guid userId)
    {
        //Criteria
        AndCriteria(x => !x.IsDeleted);
        AndCriteria(x => x.UserId == userId 
                    && x.TenantId == currentTenant.TenantId!.Value
                    && x.LeftAt == null);

        //Includes
        AddInclude(x => x.User);

        EnableTracking(); // ⭐ cần để SoftDelete
    }
}