using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Specifications;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Supplier.Specifications.Queries;
public class SupplierCountSpec
    : TenantSpecification<Domain.Entities.MultiTenants.Categories.Supplier, Guid>
{
    public SupplierCountSpec(
        ICurrentTenant currentTenant,
        string? searchTerm)
        : base(currentTenant)
    {
        AndCriteria(x => !x.IsDeleted);

        if (!string.IsNullOrEmpty(searchTerm))
        {
            AndCriteria(x =>!string.IsNullOrEmpty(x.Name) && x.Name.Contains(searchTerm));
        }

    }
}
