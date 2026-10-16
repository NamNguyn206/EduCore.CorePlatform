using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Domain.Common.Events;

/// <summary>
/// Base class cho integration events (events published to external systems)
/// </summary>
public abstract class IntegrationEvent
{
    public Guid EventId { get; }
    public DateTime OccurredOn { get; }
    public string EventType => GetType().Name;
    public Guid CorrelationId { get; }
    public Guid? TenantId { get; protected set; }
    public int Version { get; } = 1;

    protected IntegrationEvent()
    {
        EventId = Guid.NewGuid();
        OccurredOn = DateTime.UtcNow;
        CorrelationId = Guid.NewGuid();
    }

    protected IntegrationEvent(Guid correlationId, Guid? tenantId = null)
    {
        EventId = Guid.NewGuid();
        OccurredOn = DateTime.UtcNow;
        CorrelationId = correlationId;
        TenantId = tenantId;
    }

    public virtual string ToJson()
    {
        return System.Text.Json.JsonSerializer.Serialize(this, GetType());
    }
}
