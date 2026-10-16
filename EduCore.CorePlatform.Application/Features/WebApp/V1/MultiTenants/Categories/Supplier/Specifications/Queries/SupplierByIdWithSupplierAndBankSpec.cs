using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Specifications;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Supplier.Specifications.Queries;

public sealed class SupplierByIdWithSupplierAndBankSpec
    : TenantSpecification<Domain.Entities.MultiTenants.Categories.Supplier, Guid>
{
    public SupplierByIdWithSupplierAndBankSpec(ICurrentTenant currentTenant, Guid id)
        : base(currentTenant)
    {
        //Criteria
        AndCriteria(x => !x.IsDeleted);
        AndCriteria(x => x.Id == id);

        //Include
        AddInclude(x => x.SupplyCategory);
        AddInclude(x => x.Bank);
    }
}
