using EduCore.CorePlatform.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Domain.Entities.Identity;

namespace EduCore.CorePlatform.Application.Features.MobileApp.V1.Identities.Users.Specifications.Commands;

public sealed class UserByIdForSwitchTenantSpec
    : Specification<User>
{
    public UserByIdForSwitchTenantSpec(Guid id)
    {
        //Criteria
        AndCriteria(x => !x.IsDeleted);
        AndCriteria(x => x.Id == id);

        //Includes
        AddInclude(x => x.UserTenants);
    }
}