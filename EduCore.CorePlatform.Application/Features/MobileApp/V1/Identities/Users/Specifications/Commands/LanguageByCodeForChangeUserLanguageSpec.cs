using EduCore.CorePlatform.Domain.Entities.Localization;
using EduCore.CorePlatform.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Features.MobileApp.V1.Identities.Users.Specifications.Commands;

public sealed class LanguageByCodeForChangeUserLanguageSpec
    : Specification<Language>
{
    public LanguageByCodeForChangeUserLanguageSpec(string code)
    {
        AndCriteria(x => !x.IsDeleted);
        AndCriteria(x => x.Code == code);
    }
}