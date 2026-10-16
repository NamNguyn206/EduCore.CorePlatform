using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Domain.Specifications;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.Localization.Language.Specifications.Queries;

public class LanguageCountSpec
    : Specification<Domain.Entities.Localization.Language>
{
    public LanguageCountSpec(string? keyword)
    {
        AndCriteria(x => !x.IsDeleted);

        if (!string.IsNullOrEmpty(keyword))
        {
            AndCriteria(x => !string.IsNullOrEmpty(x.Name)
                            && x.Name.Contains(keyword));
        }

    }
}
