using EduCore.CorePlatform.Domain.Common.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Domain.Events.Domain.Tenant;

public class TenantSuspendedEvent : DomainEvent
{
    public Guid TenantId { get; }
    public string Reason { get; }
    public DateTime SuspendedAt { get; }
    public Guid? SuspendedByUserId { get; }

    public TenantSuspendedEvent(Guid tenantId, string reason, DateTime suspendedAt, Guid? suspendedByUserId = null)
    {
        TenantId = tenantId;
        Reason = reason;
        SuspendedAt = suspendedAt;
        SuspendedByUserId = suspendedByUserId;
    }
}