using EduCore.CorePlatform.Domain.Common.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Domain.Events.Domain.Tenant;

public class TenantActivatedEvent : DomainEvent
{
    public Guid TenantId { get; }
    public DateTime ActivatedAt { get; }
    public Guid? ActivatedByUserId { get; }

    public TenantActivatedEvent(Guid tenantId, DateTime activatedAt, Guid? activatedByUserId = null)
    {
        TenantId = tenantId;
        ActivatedAt = activatedAt;
        ActivatedByUserId = activatedByUserId;
    }
}