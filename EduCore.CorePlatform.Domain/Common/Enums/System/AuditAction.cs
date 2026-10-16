using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Domain.Common.Enums.System;

public enum AuditAction
{
    Create = 1,
    Update = 2,
    Delete = 3,
    SoftDelete = 4,
    View = 5,
    Restore = 6,
    Export = 7,
    Import = 8,
    Login = 9,
    Logout = 10,
    AccessDenied = 11,
    PermissionChanged = 12,
    TenantChanged = 13,
    System = 14
}
