using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Specifications;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Domain.Specifications;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.Categories.Country.Specifications.Commands;

public sealed class CountryForImportFromExcelSpec: Specification<Domain.Entities.System.Categories.Country>
{
    public CountryForImportFromExcelSpec()
    {
        AndCriteria(x => !x.IsDeleted);
        EnableTracking(); // ⭐ cần để update hàng loạt
    }
}
