using EduCore.CorePlatform.Domain.Common.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Domain.Events.Domain.Role;

public class RolePermissionsUpdatedEvent : DomainEvent
{
    public Guid RoleId { get; }
    public string RoleName { get; }

    public RolePermissionsUpdatedEvent(Guid roleId, string roleName)
    {
        RoleId = roleId;
        RoleName = roleName;
    }
}