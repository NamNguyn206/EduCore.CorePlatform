using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Domain.Entities.Localization;
using EduCore.CorePlatform.Domain.Entities.MultiTenants;
using EduCore.CorePlatform.Domain.Specifications;

namespace EduCore.CorePlatform.Application.Features.Tenants.Specifications.Commands;

public sealed class LanguageByCodeForChangeTenantDefaultLanguageSpec
    : Specification<Language>
{
    public LanguageByCodeForChangeTenantDefaultLanguageSpec(string code)
    {
        AndCriteria(x => !x.IsDeleted);
        AndCriteria(x => x.Code == code);
    }
}