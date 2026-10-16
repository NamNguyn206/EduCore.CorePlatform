using EduCore.CorePlatform.Domain.Common.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Domain.Events.Domain.Audit;
public class EntityChangedEvent : DomainEvent
{
    public string EntityType { get; }
    public Guid EntityId { get; }
    public string ChangeType { get; }
    public Guid? ChangedBy { get; }
    public Guid? TenantId { get; }

    public EntityChangedEvent(string entityType, Guid entityId, string changeType,
                             Guid? changedBy, Guid? tenantId)
    {
        EntityType = entityType;
        EntityId = entityId;
        ChangeType = changeType;
        ChangedBy = changedBy;
        TenantId = tenantId;
    }
}