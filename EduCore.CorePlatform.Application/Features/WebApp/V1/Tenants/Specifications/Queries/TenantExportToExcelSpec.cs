using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Domain.Specifications;
using EduCore.CorePlatform.Domain.Entities.MultiTenants;

namespace EduCore.CorePlatform.Application.Features.Tenants.Specifications.Queries;

public sealed class TenantExportToExcelSpec
    : Specification<Tenant>
{
    public TenantExportToExcelSpec(string? keyword)
    {
        AndCriteria(x => !x.IsDeleted);

        if (!string.IsNullOrEmpty(keyword))
        {
            AndCriteria(x => !string.IsNullOrEmpty(x.Name)
                            && x.Name.Contains(keyword));
        }

        ApplyOrderBy(x => x.Name);
    }
}
