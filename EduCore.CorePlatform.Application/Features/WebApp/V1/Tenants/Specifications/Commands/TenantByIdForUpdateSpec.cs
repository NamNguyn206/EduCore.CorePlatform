using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Domain.Specifications;
using EduCore.CorePlatform.Domain.Entities.MultiTenants;

namespace EduCore.CorePlatform.Application.Features.Tenants.Specifications.Commands;

public class TenantByIdForUpdateSpec
    : Specification<Tenant>
{
    public TenantByIdForUpdateSpec(Guid id)
    {
        AndCriteria(x => !x.IsDeleted);
        AndCriteria(x => x.Id == id);

        EnableTracking();
    }
}
