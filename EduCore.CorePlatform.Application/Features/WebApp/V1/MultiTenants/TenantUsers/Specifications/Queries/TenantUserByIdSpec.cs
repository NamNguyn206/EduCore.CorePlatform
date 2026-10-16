using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Specifications;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Domain.Specifications;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.TenantUsers.Specifications.Queries;

public sealed class TenantUserByIdSpec
    : Specification<Domain.Entities.Identity.User>
{
    public TenantUserByIdSpec(ICurrentTenant currentTenant, Guid id)
    {
        //Criteria
        AndCriteria(x => !x.IsDeleted);
        AndCriteria(x => x.UserTenants.Any(m => m.TenantId == currentTenant.TenantId!.Value
                                                && m.LeftAt == null));
        AndCriteria(x => x.Id == id);

        //Includes
        AddInclude(x => x.UserTenants);
        AddInclude("UserTenants.Roles");
    }
}