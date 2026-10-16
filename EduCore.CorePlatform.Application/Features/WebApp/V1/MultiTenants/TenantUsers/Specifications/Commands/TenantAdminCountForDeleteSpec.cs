using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Domain.Entities.MultiTenants.Identities;
using EduCore.CorePlatform.Domain.Specifications;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.TenantUsers.Specifications.Commands;

public class TenantAdminCountForDeleteSpec
     : Specification<UserTenant>
{
    public TenantAdminCountForDeleteSpec(ICurrentTenant currentTenant)
    {
        //Criteria
        AndCriteria(x => !x.IsDeleted);
        AndCriteria(x => x.TenantId == currentTenant.TenantId!.Value
                    && x.LeftAt == null
                    && x.IsTenantAdmin);
    }
}