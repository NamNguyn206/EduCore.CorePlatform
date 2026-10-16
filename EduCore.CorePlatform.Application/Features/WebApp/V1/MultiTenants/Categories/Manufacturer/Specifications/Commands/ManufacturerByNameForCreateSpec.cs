using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Specifications;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Manufacturer.Specifications.Commands;

public sealed class ManufacturerByNameForCreateSpec
    : TenantSpecification<Domain.Entities.MultiTenants.Categories.Manufacturer, Guid>
{
    public ManufacturerByNameForCreateSpec(ICurrentTenant currentTenant, string name)
        : base(currentTenant)
    {
        AndCriteria(x => !x.IsDeleted);
        AndCriteria(x => x.Name == name);
    }
}