using EduCore.CorePlatform.Domain.Common.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Domain.Events.Domain.Permission;

public class PermissionsSyncedEvent : DomainEvent
{
    public int NewPermissionsCount { get; }
    public int UpdatedPermissionsCount { get; }

    public PermissionsSyncedEvent(int newPermissionsCount, int updatedPermissionsCount)
    {
        NewPermissionsCount = newPermissionsCount;
        UpdatedPermissionsCount = updatedPermissionsCount;
    }
}