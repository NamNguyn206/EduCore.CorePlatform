using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Specifications;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Unit.Specifications.Queries;

public class SearchUnitSpec
    : TenantSpecification<Domain.Entities.MultiTenants.Categories.Unit, Guid>
{
    public SearchUnitSpec(
        ICurrentTenant currentTenant,
        string? searchTerm,
        int take = 10
    ) : base(currentTenant)
    {
        AndCriteria(x => !x.IsDeleted);

        if (!string.IsNullOrEmpty(searchTerm))
        {
            AndCriteria(x => !string.IsNullOrEmpty(x.Name)
                            && x.Name.Contains(searchTerm));
        }

        take = Math.Min(take, 20);

        ApplyPaging(0, take);

        ApplyOrderBy(x => x.Name);
    }
}
