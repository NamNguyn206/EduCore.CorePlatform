using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Specifications;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Department.Specifications.Commands;

public sealed class DepartmentForImportFromExcelSpec
    : TenantSpecification<Domain.Entities.MultiTenants.Categories.Department, Guid>
{
    public DepartmentForImportFromExcelSpec(ICurrentTenant currentTenant)
        : base(currentTenant)
    {
        AndCriteria(x => !x.IsDeleted);
        EnableTracking(); // ⭐ cần để update hàng loạt
    }
}
