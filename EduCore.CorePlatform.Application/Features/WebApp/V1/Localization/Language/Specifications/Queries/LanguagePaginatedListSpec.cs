using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Domain.Specifications;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.Localization.Language.Specifications.Queries;

public class LanguagePaginatedListSpec
    : Specification<Domain.Entities.Localization.Language>
{
    public LanguagePaginatedListSpec(
        string? keyword,
        int pageIndex,
        int searchTerm
    )
    {
        AndCriteria(x => !x.IsDeleted);

        if (!string.IsNullOrEmpty(keyword))
        {
            AndCriteria(x => !string.IsNullOrEmpty(x.Name)
                            && x.Name.Contains(keyword));
        }

        ApplyOrderBy(x => x.Name);
        ApplyPaging((pageIndex - 1) * searchTerm, searchTerm);
    }
}
