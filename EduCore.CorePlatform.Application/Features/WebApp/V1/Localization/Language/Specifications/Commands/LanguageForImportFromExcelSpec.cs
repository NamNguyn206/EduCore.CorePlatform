using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Domain.Specifications;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.Localization.Language.Specifications.Commands;
 
public sealed class LanguageForImportFromExcelSpec : Specification<Domain.Entities.Localization.Language>
{
    public LanguageForImportFromExcelSpec()
    {
        AndCriteria(x => !x.IsDeleted);
        EnableTracking(); // ⭐ cần để update hàng loạt
    }
}
