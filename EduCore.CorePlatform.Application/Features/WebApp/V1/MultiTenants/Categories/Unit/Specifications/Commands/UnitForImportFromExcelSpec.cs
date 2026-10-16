using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Specifications;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Unit.Specifications.Commands;

public sealed class UnitForImportFromExcelSpec
    : TenantSpecification<Domain.Entities.MultiTenants.Categories.Unit, Guid>
{
    public UnitForImportFromExcelSpec(ICurrentTenant currentTenant)
        : base(currentTenant)
    {
        AndCriteria(x => !x.IsDeleted);
        EnableTracking(); // ⭐ cần để update hàng loạt
    }
}
