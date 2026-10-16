using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Domain.Specifications;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.Localization.Language.Specifications.Commands;

public class LanguageByIdForUpdateSpec
    : Specification<Domain.Entities.Localization.Language>
{
    public LanguageByIdForUpdateSpec(Guid id)
    {
        AndCriteria(x => !x.IsDeleted);
        AndCriteria(x => x.Id == id);

        EnableTracking();
    }
}
