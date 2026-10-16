using EduCore.CorePlatform.Domain.Common.Constants;
using EduCore.CorePlatform.Domain.Common.Entities;
using EduCore.CorePlatform.Domain.Common.Enums.System.Webhook;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EduCore.CorePlatform.Domain.Entities.System.Webhooks;

public class WebhookSubscription : MultiTenantEntity<Guid, Guid>
{
    public Guid WebhookDefinitionId { get; private set; }    
    public WebhookEventType EventType { get; private set; }
    public string? EntityType { get; private set; }
    public Guid? EntityId { get; private set; }
    public string? Filters { get; private set; } // JSON filter conditions
    public bool IsActive { get; private set; } = true;
    public DateTime SubscribedAt { get; private set; }
    public DateTime? UnsubscribedAt { get; private set; }
    public string? CallbackUrl { get; private set; }
    public string? Secret { get; private set; }

    // Navigation properties
    public virtual WebhookDefinition WebhookDefinition { get; private set; } = null!;

    protected WebhookSubscription() { }

    public WebhookSubscription(Guid tenantId, Guid webhookDefinitionId, WebhookEventType eventType,
                              string? entityType = null, Guid? entityId = null, string? callbackUrl = null)
        : base(Guid.NewGuid(), tenantId)
    {
        WebhookDefinitionId = webhookDefinitionId;
        EventType = eventType;
        EntityType = entityType;
        EntityId = entityId;
        CallbackUrl = callbackUrl;
        SubscribedAt = DateTime.UtcNow;
        IsActive = true;
        Secret = Guid.NewGuid().ToString("N");
    }

    public void UpdateFilters(string? filters)
    {
        Filters = filters;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        UnsubscribedAt = null;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UnsubscribedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetCallbackUrl(string callbackUrl)
    {
        CallbackUrl = callbackUrl;
        UpdatedAt = DateTime.UtcNow;
    }

    public void RegenerateSecret()
    {
        Secret = Guid.NewGuid().ToString("N");
        UpdatedAt = DateTime.UtcNow;
    }

    public bool Matches(WebhookEvent webhookEvent)
    {
        if (EventType != webhookEvent.EventType)
            return false;

        if (!string.IsNullOrEmpty(EntityType) && EntityType != webhookEvent.EntityType)
            return false;

        if (EntityId.HasValue && EntityId != webhookEvent.EntityId)
            return false;

        // TODO: Apply filter logic if Filters is set
        return true;
    }
}