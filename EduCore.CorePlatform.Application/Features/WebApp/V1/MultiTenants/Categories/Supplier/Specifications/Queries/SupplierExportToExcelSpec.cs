using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Application.Common.Specifications;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Supplier.Specifications.Queries;

public sealed class SupplierExportToExcelSpec
    : TenantSpecification<Domain.Entities.MultiTenants.Categories.Supplier, Guid>
{
    public SupplierExportToExcelSpec(ICurrentTenant currentTenant, string? searchTerm)
        : base(currentTenant)
    {
        AndCriteria(x => !x.IsDeleted);

        if (!string.IsNullOrEmpty(searchTerm))
        {
            AndCriteria(x => !string.IsNullOrEmpty(x.Name) && x.Name.Contains(searchTerm));
        }

        ApplyOrderBy(x => x.Name);
    }
}
