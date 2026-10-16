using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Domain.Common.Enums;
using EduCore.CorePlatform.Domain.Entities.System.Messages;

namespace EduCore.CorePlatform.Domain.Specifications.System.SystemMessages;

public sealed class SystemMessageByCodeWithLanguageSpec
    : Specification<SystemMessage>
{
    public SystemMessageByCodeWithLanguageSpec(string code)
    {
        AndCriteria(x => x.Code == code);
        AddInclude(x => x.Translations);
    }
}