using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Specifications;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.SupplyCategory.Specifications.Commands;

public class SupplyCategoryListForUpdateSpec
    : TenantSpecification<Domain.Entities.MultiTenants.Categories.SupplyCategory, Guid>
{
    public SupplyCategoryListForUpdateSpec(
        ICurrentTenant currentTenant,
        string? keyword)
        : base(currentTenant)
    {
        AndCriteria(x => !x.IsDeleted);

        if (!string.IsNullOrEmpty(keyword))
        {
            AndCriteria(x => !string.IsNullOrEmpty(x.Name)
                            && x.Name.Contains(keyword));
        }

        ApplyOrderBy(x => x.Name);
        EnableTracking();
    }
}
