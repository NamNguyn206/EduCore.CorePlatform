using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Domain.Entities.MultiTenants;
using EduCore.CorePlatform.Domain.Specifications;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.Identities.Users.Specifications.Queries;

public sealed class UserByIdForLanguageSpec : Specification<Domain.Entities.Identity.User>
{
    public UserByIdForLanguageSpec(Guid id)
    {
        //Criteria
        AndCriteria(x => !x.IsDeleted);
        AndCriteria(x => x.Id == id);

        //include
        AddInclude(x => x.Language);
    }
}
