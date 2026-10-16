using EduCore.CorePlatform.Domain.Common.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Domain.Events.Domain.Role;

public class RoleCreatedEvent : DomainEvent
{
    public Guid RoleId { get; }
    public string RoleName { get; }
    public Guid? TenantId { get; }

    public RoleCreatedEvent(Guid roleId, string roleName, Guid? tenantId)
    {
        RoleId = roleId;
        RoleName = roleName;
        TenantId = tenantId;
    }
}