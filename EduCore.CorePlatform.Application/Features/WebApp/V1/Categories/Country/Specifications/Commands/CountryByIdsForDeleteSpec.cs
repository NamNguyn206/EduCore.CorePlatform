using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Specifications;
using EduCore.CorePlatform.Domain.Specifications;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.Categories.Country.Specifications.Commands;

public sealed class CountryByIdsForDeleteSpec
    : Specification<Domain.Entities.System.Categories.Country>
{
    public CountryByIdsForDeleteSpec(IReadOnlyCollection<Guid> ids)
    {
        AndCriteria(x => !x.IsDeleted);
        AndCriteria(x => ids.Contains(x.Id));

        EnableTracking(); // ⭐ cần để SoftDelete
    }
}
