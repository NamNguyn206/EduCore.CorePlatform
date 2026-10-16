using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Domain.Entities.MultiTenants;
using EduCore.CorePlatform.Domain.Specifications;

namespace EduCore.CorePlatform.Application.Features.Tenants.Specifications.Commands;

public class TenantByIdForChangeDefaultLanguageSpec: Specification<Tenant>
{
    public TenantByIdForChangeDefaultLanguageSpec(Guid id)
    {
        AndCriteria(x => !x.IsDeleted);
        AndCriteria(x => x.Id == id);

        EnableTracking();
    }
}
