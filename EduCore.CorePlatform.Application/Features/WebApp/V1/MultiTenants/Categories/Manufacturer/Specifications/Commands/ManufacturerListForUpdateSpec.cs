using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Specifications;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Manufacturer.Specifications.Commands;

public class ManufacturerListForUpdateSpec
    : TenantSpecification<Domain.Entities.MultiTenants.Categories.Manufacturer, Guid>
{
    public ManufacturerListForUpdateSpec(
        ICurrentTenant currentTenant,
        string? keyword)
        : base(currentTenant)
    {
        AndCriteria(x => !x.IsDeleted);

        if (!string.IsNullOrEmpty(keyword))
        {
            AndCriteria(x => !string.IsNullOrEmpty(x.Name)
                            && x.Name.Contains(keyword));
        }

        ApplyOrderBy(x => x.Name);
        EnableTracking();
    }
}