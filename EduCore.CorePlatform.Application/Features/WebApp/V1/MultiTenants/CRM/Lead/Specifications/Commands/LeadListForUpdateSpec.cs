using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Application.Common.Specifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.CRM.Lead.Specifications.Commands;

public class LeadListForUpdateSpec
    : TenantSpecification<Domain.Entities.MultiTenants.CRM.Lead, Guid>
{
    public LeadListForUpdateSpec(
        ICurrentTenant currentTenant,
        string? keyword)
        : base(currentTenant)
    {
        AndCriteria(x => !x.IsDeleted);

        if (!string.IsNullOrEmpty(keyword))
        {
            AndCriteria(x => (!string.IsNullOrEmpty(x.FullName) && x.FullName.Contains(keyword))
                          || (!string.IsNullOrEmpty(x.Email) && x.Email.Contains(keyword))
                          || (!string.IsNullOrEmpty(x.PhoneNumber) && x.PhoneNumber.Contains(keyword)));
        }

        ApplyOrderBy(x => x.FullName);
        EnableTracking();
    }
}