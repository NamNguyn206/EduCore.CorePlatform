using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Domain.Specifications;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.Categories.Country.Specifications.Queries;

public class CountryPaginatedListSpec
    : Specification<Domain.Entities.System.Categories.Country>
{
    public CountryPaginatedListSpec(
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
