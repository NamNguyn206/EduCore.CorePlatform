using EduCore.CorePlatform.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Features.MobileApp.V1.Identities.Users.Specifications.Commands;

public class UserByIdForChangeLanguageSpec : Specification<Domain.Entities.Identity.User>
{
    public UserByIdForChangeLanguageSpec(Guid id)
    {
        AndCriteria(x => !x.IsDeleted);
        AndCriteria(x => x.Id == id);

        EnableTracking();
    }
}
