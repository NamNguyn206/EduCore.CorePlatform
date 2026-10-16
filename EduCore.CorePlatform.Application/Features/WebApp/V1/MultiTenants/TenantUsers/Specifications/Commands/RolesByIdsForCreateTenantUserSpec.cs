using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Domain.Common.Enums;
using EduCore.CorePlatform.Domain.Entities.Identity;
using EduCore.CorePlatform.Domain.Specifications;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.TenantUsers.Specifications.Commands;

public class RolesByIdsForCreateTenantUserSpec : Specification<Role>
{
    public RolesByIdsForCreateTenantUserSpec(List<Guid> roleIds)
    {
        //Criteria
        AndCriteria(x => !x.IsDeleted);
        AndCriteria(x => roleIds.Contains(x.Id));
    }
}