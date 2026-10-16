using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Domain.Specifications;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.Categories.Country.Specifications.Queries;

public sealed class CountryByIdSpec
    : Specification<Domain.Entities.System.Categories.Country>
{
    public CountryByIdSpec(Guid id)
    {
        AndCriteria(x => !x.IsDeleted);
        AndCriteria(x => x.Id == id);
    }
}
