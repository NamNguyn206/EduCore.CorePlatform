using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Domain.Specifications;

namespace EduCore.CorePlatform.Application.Features.MobileApp.V1.Localization.Language.Specifications.Queries;

public class LanguageListSpec
    : Specification<Domain.Entities.Localization.Language>
{
    public LanguageListSpec(string? keyword)
    {
        AndCriteria(x => !x.IsDeleted);

        if (!string.IsNullOrEmpty(keyword))
        {
            AndCriteria(x => !string.IsNullOrEmpty(x.Name)
                            && x.Name.Contains(keyword));
        }

        ApplyOrderBy(x => x.Name);
    }
}
