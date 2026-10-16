using EduCore.CorePlatform.Domain.Common.Constants;
using EduCore.CorePlatform.Domain.Common.Entities;
using EduCore.CorePlatform.Domain.Common.Enums.System.Webhook;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace EduCore.CorePlatform.Domain.Entities.System.Webhooks;

public class WebhookEvent : MultiTenantEntity<Guid, Guid>
{
    public WebhookEventType EventType { get; private set; }
    public string EntityType { get; private set; } = string.Empty;
    public Guid? EntityId { get; private set; }
    public string? Payload { get; private set; }
    public DateTime OccurredAt { get; private set; }
    public Guid? TriggeredBy { get; private set; }
    public string? TriggeredByName { get; private set; }
    public bool IsProcessed { get; private set; }
    public DateTime? ProcessedAt { get; private set; }
    public string? Metadata { get; private set; }

    // Navigation properties
    public virtual ICollection<WebhookLog> WebhookLogs { get; private set; } = new List<WebhookLog>();

    protected WebhookEvent() { }

    public WebhookEvent(Guid tenantId, WebhookEventType eventType, string entityType,
                       Guid? entityId, string? payload, Guid? triggeredBy = null,
                       string? triggeredByName = null, string? metadata = null)
        : base(Guid.NewGuid(), tenantId)
    {
        EventType = eventType;
        EntityType = entityType;
        EntityId = entityId;
        Payload = payload;
        OccurredAt = DateTime.UtcNow;
        TriggeredBy = triggeredBy;
        TriggeredByName = triggeredByName;
        Metadata = metadata;
        IsProcessed = false;
    }

    public void MarkAsProcessed()
    {
        IsProcessed = true;
        ProcessedAt = DateTime.UtcNow;
    }

    public void SetPayload(object payload)
    {
        Payload = JsonSerializer.Serialize(payload);
    }

    public T? GetPayload<T>()
    {
        if (string.IsNullOrEmpty(Payload))
            return default;

        return JsonSerializer.Deserialize<T>(Payload);
    }
}