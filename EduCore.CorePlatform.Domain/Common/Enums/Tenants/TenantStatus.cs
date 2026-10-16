using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Domain.Common.Enums.Tenants;

public enum TenantStatus
{
    Active = 1,
    Suspended = 2,
    Pending = 3,
    Expired = 4,
    Deleted = 5
}