using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Specifications;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Supplier.Specifications.Commands;

public sealed class SupplierForImportFromExcelSpec
    : TenantSpecification<Domain.Entities.MultiTenants.Categories.Supplier, Guid>
{
    public SupplierForImportFromExcelSpec(ICurrentTenant currentTenant)
        : base(currentTenant)
    {
        AndCriteria(x => !x.IsDeleted);
        EnableTracking(); // ⭐ cần để update hàng loạt
    }
}
