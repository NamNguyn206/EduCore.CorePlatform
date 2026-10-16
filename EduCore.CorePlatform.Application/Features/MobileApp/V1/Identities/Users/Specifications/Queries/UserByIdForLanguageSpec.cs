using EduCore.CorePlatform.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Features.MobileApp.V1.Identities.Users.Specifications.Queries;

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
