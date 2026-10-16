using EduCore.CorePlatform.Domain.Common.Enums.System.Webhook;
using EduCore.CorePlatform.Domain.Common.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Domain.Events.Domain.Webhook;

public class WebhookTriggeredEvent : DomainEvent
{
    public Guid WebhookDefinitionId { get; }
    public Guid WebhookEventId { get; }
    public WebhookEventType EventType { get; }
    public Guid TenantId { get; }

    public WebhookTriggeredEvent(Guid webhookDefinitionId, Guid webhookEventId,
                                WebhookEventType eventType, Guid tenantId)
    {
        WebhookDefinitionId = webhookDefinitionId;
        WebhookEventId = webhookEventId;
        EventType = eventType;
        TenantId = tenantId;
    }
}