using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Specifications;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Manufacturer.Specifications.Queries;

public class ManufacturerPaginatedListSpec
    : TenantSpecification<Domain.Entities.MultiTenants.Categories.Manufacturer, Guid>
{
    public ManufacturerPaginatedListSpec(
        ICurrentTenant currentTenant,
        string? searchTerm,
        int pageIndex,
        int pageSize)
        : base(currentTenant)
    {
        AndCriteria(x => !x.IsDeleted);

        if (!string.IsNullOrEmpty(searchTerm))
        {
            AndCriteria(x => !string.IsNullOrEmpty(x.Name)
                            && x.Name.Contains(searchTerm));
        }

        ApplyOrderBy(x => x.Name);
        ApplyPaging((pageIndex - 1) * pageSize, pageSize);
    }
}
