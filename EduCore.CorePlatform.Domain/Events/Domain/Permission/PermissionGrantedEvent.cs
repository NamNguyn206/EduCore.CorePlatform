using EduCore.CorePlatform.Domain.Common.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Domain.Events.Domain.Permission;

public class PermissionGrantedEvent : DomainEvent
{
    public Guid PermissionId { get; }
    public Guid RoleId { get; }
    public Guid? TenantId { get; }

    public PermissionGrantedEvent(Guid permissionId, Guid roleId, Guid? tenantId = null)
    {
        PermissionId = permissionId;
        RoleId = roleId;
        TenantId = tenantId;
    }
}