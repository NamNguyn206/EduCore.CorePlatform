using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Domain.Common.Enums.System.MobileApp;

public enum MobileAppSyncOperation
{
    Create = 1,
    Update = 2,
    Delete = 3,
    Sync = 4,
    Conflict = 5,
    Resolved = 6
}