using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Domain.Specifications;

namespace EduCore.CorePlatform.Infrastructure.Persistence.Seeders.Bootstrap.Permission;

public sealed class PermissionCodesSpec
    : Specification<Domain.Entities.Identity.Permission>
{
    public PermissionCodesSpec(IEnumerable<string> codes)
    {
        AndCriteria(p => codes.Contains(p.Code)
                        && !p.IsDeleted);
    }
}