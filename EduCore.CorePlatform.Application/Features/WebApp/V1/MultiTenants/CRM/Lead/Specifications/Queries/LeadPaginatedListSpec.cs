using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Application.Common.Specifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.CRM.Lead.Specifications.Queries;

public class LeadPaginatedListSpec
    : TenantSpecification<Domain.Entities.MultiTenants.CRM.Lead, Guid>
{
    public LeadPaginatedListSpec(
        ICurrentTenant currentTenant,
        string? searchTerm,
        int pageIndex,
        int pageSize)
        : base(currentTenant)
    {
        AndCriteria(x => !x.IsDeleted);

        if (!string.IsNullOrEmpty(searchTerm))
        {
            AndCriteria(x => (!string.IsNullOrEmpty(x.FullName) && x.FullName.Contains(searchTerm))
                          || (!string.IsNullOrEmpty(x.Email) && x.Email.Contains(searchTerm))
                          || (!string.IsNullOrEmpty(x.PhoneNumber) && x.PhoneNumber.Contains(searchTerm)));
        }

        ApplyOrderBy(x => x.FullName);
        ApplyPaging((pageIndex - 1) * pageSize, pageSize);
    }
}

