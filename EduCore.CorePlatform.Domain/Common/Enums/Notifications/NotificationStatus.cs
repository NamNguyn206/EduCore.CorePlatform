using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Domain.Common.Enums.Notifications;
  
public enum NotificationStatus
{
    Pending = 1,
    Sent = 2,
    Read = 3,
    Failed = 4,
    Cancelled = 5
}