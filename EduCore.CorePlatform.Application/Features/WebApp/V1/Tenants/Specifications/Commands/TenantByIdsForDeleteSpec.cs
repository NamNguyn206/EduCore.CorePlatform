using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Domain.Specifications;
using EduCore.CorePlatform.Domain.Entities.MultiTenants;

namespace EduCore.CorePlatform.Application.Features.Tenants.Specifications.Commands;

public sealed class TenantByIdsForDeleteSpec
    : Specification<Tenant>
{
    public TenantByIdsForDeleteSpec(IReadOnlyCollection<Guid> ids)
    {
        AndCriteria(x => !x.IsDeleted);
        AndCriteria(x => ids.Contains(x.Id));

        EnableTracking(); // ⭐ cần để SoftDelete
    }
}
