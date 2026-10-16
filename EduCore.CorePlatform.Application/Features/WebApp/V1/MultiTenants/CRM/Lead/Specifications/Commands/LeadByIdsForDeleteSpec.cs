using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Application.Common.Specifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.CRM.Lead.Specifications.Commands;

public sealed class LeadByIdsForDeleteSpec
    : TenantSpecification<Domain.Entities.MultiTenants.CRM.Lead, Guid>
{
    public LeadByIdsForDeleteSpec(ICurrentTenant currentTenant,
                                    IReadOnlyCollection<Guid> ids)
        : base(currentTenant)
    {
        AndCriteria(x => !x.IsDeleted);
        AndCriteria(x => ids.Contains(x.Id));

        EnableTracking(); // ⭐ cần để SoftDelete
    }
}