using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Domain.Specifications;
using EduCore.CorePlatform.Domain.Entities.Localization;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.Localization.Language.Specifications.Queries;

public class LanguageListForUpdateSpec
    : Specification<Domain.Entities.Localization.Language>
{
    public LanguageListForUpdateSpec(string? keyword)
    {
        AndCriteria(x => !x.IsDeleted);

        if (!string.IsNullOrEmpty(keyword))
        {
            AndCriteria(x => !string.IsNullOrEmpty(x.Name)
                            && x.Name.Contains(keyword));
        }

        ApplyOrderBy(x => x.Name);
        EnableTracking();
    }
}