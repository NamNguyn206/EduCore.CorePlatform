using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Domain.Specifications;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.TenantUsers.Specifications.Commands;

public sealed class UserByEmailForCreateTenantUserSpec
    : Specification<Domain.Entities.Identity.User>
{
    public UserByEmailForCreateTenantUserSpec(string email)
    {
        //Criteria
        AndCriteria(x => !x.IsDeleted);
        AndCriteria(x => x.Email == email);

        //Includes
        AddInclude(x => x.UserTenants);
    }
}