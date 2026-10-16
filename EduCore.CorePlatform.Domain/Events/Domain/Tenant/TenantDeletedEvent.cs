using EduCore.CorePlatform.Domain.Common.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Domain.Events.Domain.Tenant;

public class TenantDeletedEvent : DomainEvent
{
    public Guid TenantId { get; }
    public string TenantName { get; }
    public DateTime DeletedAt { get; }
    public Guid? DeletedByUserId { get; }

    public TenantDeletedEvent(Guid tenantId, string tenantName, DateTime deletedAt, Guid? deletedByUserId = null)
    {
        TenantId = tenantId;
        TenantName = tenantName;
        DeletedAt = deletedAt;
        DeletedByUserId = deletedByUserId;
    }
}
