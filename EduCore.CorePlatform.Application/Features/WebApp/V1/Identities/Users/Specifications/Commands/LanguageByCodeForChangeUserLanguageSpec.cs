using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Domain.Entities.Localization;
using EduCore.CorePlatform.Domain.Specifications;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.Identities.Users.Specifications.Commands;

public sealed class LanguageByCodeForChangeUserLanguageSpec
    : Specification<Language>
{
    public LanguageByCodeForChangeUserLanguageSpec(string code)
    {
        AndCriteria(x => !x.IsDeleted);
        AndCriteria(x => x.Code == code);
    }
}