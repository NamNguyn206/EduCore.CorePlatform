using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Domain.Specifications;
using EduCore.CorePlatform.Domain.Entities.MultiTenants;

namespace EduCore.CorePlatform.Application.Features.Tenants.Specifications.Commands;

public sealed class TenantByCodeForCreateSpec
    : Specification<Tenant>
{
    public TenantByCodeForCreateSpec(string code)
    {
        AndCriteria(x => !x.IsDeleted);
        AndCriteria(x => x.Code == code);
    }
}