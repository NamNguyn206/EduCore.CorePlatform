using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Application.Common.Specifications;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Supplier.Specifications.Queries;

public class SupplierPaginatedListWithSupplierAndBankSpec
    : TenantSpecification<Domain.Entities.MultiTenants.Categories.Supplier, Guid>
{
    public SupplierPaginatedListWithSupplierAndBankSpec(
                                                            ICurrentTenant currentTenant,
                                                            string? searchTerm,
                                                            int pageIndex,
                                                            int pageSize)
        : base(currentTenant)
    {
        //Criteria
        AndCriteria(x => !x.IsDeleted);

        if (!string.IsNullOrEmpty(searchTerm))
        {
            AndCriteria(x => !string.IsNullOrEmpty(x.Name) && x.Name.Contains(searchTerm));
        }

        //Include
        AddInclude(x => x.SupplyCategory);
        AddInclude(x => x.Bank);

        ApplyPaging((pageIndex - 1) * pageSize, pageSize);
    }
}