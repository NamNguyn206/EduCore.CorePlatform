using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Domain.Specifications;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.Localization.Language.Specifications.Commands;

public sealed class LanguageByIdsForDeleteSpec
    : Specification<Domain.Entities.Localization.Language>
{
    public LanguageByIdsForDeleteSpec(IReadOnlyCollection<Guid> ids)
    {
        AndCriteria(x => !x.IsDeleted);
        AndCriteria(x => ids.Contains(x.Id));

        EnableTracking(); // ⭐ cần để SoftDelete
    }
}
