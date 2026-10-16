using EduCore.CorePlatform.Domain.Common.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Domain.Events.Domain.Tenant;

public class TenantAdminAssignedEvent : DomainEvent
{
    public Guid TenantId { get; }
    public Guid UserId { get; }

    public TenantAdminAssignedEvent(Guid tenantId, Guid userId)
    {
        TenantId = tenantId;
        UserId = userId;
    }
}