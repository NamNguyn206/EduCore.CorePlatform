using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Specifications;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Supplier.Specifications.Commands;

public sealed class SupplierByIdsForDeleteSpec
    : TenantSpecification<Domain.Entities.MultiTenants.Categories.Supplier, Guid>
{
    public SupplierByIdsForDeleteSpec(ICurrentTenant currentTenant,
                                            IReadOnlyCollection<Guid> ids)
        : base(currentTenant)
    {
        AndCriteria(x => !x.IsDeleted);
        AndCriteria(x => ids.Contains(x.Id));
        EnableTracking(); // ⭐ cần để SoftDelete
    }
}
