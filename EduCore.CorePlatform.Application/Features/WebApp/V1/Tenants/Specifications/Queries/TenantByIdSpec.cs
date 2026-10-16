using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Domain.Specifications;
using EduCore.CorePlatform.Domain.Entities.MultiTenants;

namespace EduCore.CorePlatform.Application.Features.Tenants.Specifications.Queries;

public sealed class TenantByIdSpec: Specification<Tenant>
{
    public TenantByIdSpec(Guid id)
    {
        AndCriteria(x => !x.IsDeleted);
        AndCriteria(x => x.Id == id);
    }
}
