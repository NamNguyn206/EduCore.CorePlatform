using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Domain.Common.Enums.Identities;

public enum UserStatus
{
    Active = 1,
    Inactive = 2,
    Suspended = 3,
    PendingActivation = 4,
    Deleted = 5
}