using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Domain.Common.Enums.Identities;

public enum UserVerificationStatus
{
    Pending = 1,

    Verified = 2,

    Rejected = 3,

    Suspended = 4
}