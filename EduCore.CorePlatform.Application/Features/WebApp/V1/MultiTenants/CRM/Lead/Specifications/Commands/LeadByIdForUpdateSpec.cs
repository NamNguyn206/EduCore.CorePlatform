using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Application.Common.Specifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.CRM.Lead.Specifications.Commands;

public class LeadByIdForUpdateSpec
    : TenantSpecification<Domain.Entities.MultiTenants.CRM.Lead, Guid>
{
    public LeadByIdForUpdateSpec(
        ICurrentTenant currentTenant,
        Guid id)
        : base(currentTenant)
    {
        AndCriteria(x => !x.IsDeleted);
        AndCriteria(x => x.Id == id);

        EnableTracking();
    }
}