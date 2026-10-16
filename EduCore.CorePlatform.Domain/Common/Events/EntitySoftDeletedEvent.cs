using EduCore.CorePlatform.Domain.Events.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Domain.Common.Events;

public sealed class EntitySoftDeletedEvent<TId> : DomainEvent
{
    public TId EntityId { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public EntitySoftDeletedEvent(TId entityId)
    {
        EntityId = entityId;
    }
}

public sealed class EntityRestoredEvent<TId> : DomainEvent
{
    public TId EntityId { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public EntityRestoredEvent(TId entityId)
    {
        EntityId = entityId;
    }
}
