using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Specifications;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.SupplyCategory.Specifications.Queries;

public sealed class SupplyCategoryByIdSpec
    : TenantSpecification<Domain.Entities.MultiTenants.Categories.SupplyCategory, Guid>
{
    public SupplyCategoryByIdSpec(ICurrentTenant currentTenant, Guid id)
        : base(currentTenant)
    {
        AndCriteria(x => !x.IsDeleted);
        AndCriteria(x => x.Id == id);
    }
}
