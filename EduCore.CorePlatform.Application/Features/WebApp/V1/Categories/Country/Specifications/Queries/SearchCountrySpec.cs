using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Domain.Specifications;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.Categories.Country.Specifications.Queries;

public class SearchCountrySpec
    : Specification<Domain.Entities.System.Categories.Country>
{
    public SearchCountrySpec(
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
