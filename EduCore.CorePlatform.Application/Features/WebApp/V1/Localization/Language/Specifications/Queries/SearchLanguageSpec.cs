using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Specifications;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Domain.Specifications;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.Localization.Language.Specifications.Queries;

public class SearchLanguageSpec
    : Specification<Domain.Entities.Localization.Language>
{
    public SearchLanguageSpec(
        string? searchTerm,
        int take = 10
    )
    {
        AndCriteria(x => !x.IsDeleted);

        if (!string.IsNullOrEmpty(searchTerm))
        {
            AndCriteria(x => !string.IsNullOrEmpty(x.Name)
                            && x.Name.Contains(searchTerm));
        }

        take = Math.Min(take, 20);

        ApplyPaging(0, take);

        ApplyOrderBy(x => x.Name);
    }
}
