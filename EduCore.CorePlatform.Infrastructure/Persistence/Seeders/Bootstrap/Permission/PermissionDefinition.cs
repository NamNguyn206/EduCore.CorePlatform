using EduCore.CorePlatform.Domain.Common.Enums.Identities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Infrastructure.Persistence.Seeders.Bootstrap.Permission;

public sealed record PermissionDefinition(
    string Code,
    string Name,
    string Description,
    RoleScope Scope
);