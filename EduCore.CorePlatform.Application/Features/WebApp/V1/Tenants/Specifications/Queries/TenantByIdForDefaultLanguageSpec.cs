using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Domain.Entities.MultiTenants;
using EduCore.CorePlatform.Domain.Specifications;

namespace EduCore.CorePlatform.Application.Features.Tenants.Specifications.Queries;

public sealed class TenantByIdForDefaultLanguageSpec: Specification<Tenant>
{
    public TenantByIdForDefaultLanguageSpec(Guid id)
    {
        //Criteria
        AndCriteria(x => !x.IsDeleted);
        AndCriteria(x => x.Id == id);

        //Includes
        AddInclude(x => x.DefaultLanguage);
    }
}
