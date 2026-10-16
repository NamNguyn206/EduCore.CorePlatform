using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Domain.Specifications;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.Localization.Language.Specifications.Commands;

public sealed class LanguageByCodeForCreateSpec
    : Specification<Domain.Entities.Localization.Language>
{
    public LanguageByCodeForCreateSpec(string code)
    {
        AndCriteria(x => !x.IsDeleted);
        AndCriteria(x => x.Code == code);
    }
}