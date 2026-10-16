using EduCore.CorePlatform.Domain.Common.Enums.Tenants;
using EduCore.CorePlatform.Domain.Common.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Domain.Events.Domain.Tenant;

public class TenantStatusChangedEvent : DomainEvent
{
    public Guid TenantId { get; }
    public TenantStatus NewStatus { get; }
    public TenantStatus? OldStatus { get; }

    public TenantStatusChangedEvent(Guid tenantId, TenantStatus newStatus, TenantStatus? oldStatus = null)
    {
        TenantId = tenantId;
        NewStatus = newStatus;
        OldStatus = oldStatus;
    }
}