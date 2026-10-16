using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Domain.Specifications;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.Categories.Country.Specifications.Queries;

public class CountryListForUpdateSpec
    : Specification<Domain.Entities.System.Categories.Country>
{
    public CountryListForUpdateSpec(string? searchTerm)
    {
        AndCriteria(x => !x.IsDeleted);

        if (!string.IsNullOrEmpty(searchTerm))
        {
            AndCriteria(x => !string.IsNullOrEmpty(x.Name)
                            && x.Name.Contains(searchTerm));
        }

        ApplyOrderBy(x => x.Name);
        EnableTracking();
    }
}