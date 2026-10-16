using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Domain.Specifications;

namespace EduCore.CorePlatform.Application.Features.System.SystemMessages.Specifications.Commands;

public class LanguageListForImportSystemMessageFromExcelSpec
    : Specification<Domain.Entities.Localization.Language>
{
    public LanguageListForImportSystemMessageFromExcelSpec()
    {
        AndCriteria(x => !x.IsDeleted);

        EnableTracking();
    }
}
