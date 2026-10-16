using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Specifications;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Department.Specifications.Commands;

public class DepartmentByIdForUpdateSpec
    : TenantSpecification<Domain.Entities.MultiTenants.Categories.Department, Guid>
{
    public DepartmentByIdForUpdateSpec(
        ICurrentTenant currentTenant,
        Guid id)
        : base(currentTenant)
    {
        AndCriteria(x => !x.IsDeleted);
        AndCriteria(x => x.Id == id);
        EnableTracking();
    }
}
