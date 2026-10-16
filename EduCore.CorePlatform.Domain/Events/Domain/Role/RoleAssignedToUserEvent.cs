using EduCore.CorePlatform.Domain.Common.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Domain.Events.Domain.Role;

public class RoleAssignedToUserEvent : DomainEvent
{
    public Guid RoleId { get; }
    public Guid UserId { get; }
    public Guid TenantId { get; }

    public RoleAssignedToUserEvent(Guid roleId, Guid userId, Guid tenantId)
    {
        RoleId = roleId;
        UserId = userId;
        TenantId = tenantId;
    }
}