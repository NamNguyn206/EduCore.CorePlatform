using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Specifications;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Department.Specifications.Queries;

public sealed class DepartmentExportToExcelSpec
    : TenantSpecification<Domain.Entities.MultiTenants.Categories.Department, Guid>
{
    public DepartmentExportToExcelSpec(ICurrentTenant currentTenant, string? searchTerm)
        : base(currentTenant)
    {
        AndCriteria(x => !x.IsDeleted);

        if (!string.IsNullOrEmpty(searchTerm))
        {
            AndCriteria(x => (!string.IsNullOrEmpty(x.Name) && x.Name.Contains(searchTerm)
                            || (!string.IsNullOrEmpty(x.Address) && x.Address.Contains(searchTerm))));
        }

        ApplyOrderBy(x => x.Name);
    }
}
