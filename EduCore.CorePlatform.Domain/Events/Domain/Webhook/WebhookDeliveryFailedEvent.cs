using EduCore.CorePlatform.Domain.Common.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Domain.Events.Domain.Webhook;

public class WebhookDeliveryFailedEvent : DomainEvent
{
    public Guid WebhookLogId { get; }
    public Guid WebhookDefinitionId { get; }
    public string ErrorMessage { get; }
    public int AttemptNumber { get; }
    public Guid TenantId { get; }

    public WebhookDeliveryFailedEvent(Guid webhookLogId, Guid webhookDefinitionId,
                                     string errorMessage, int attemptNumber, Guid tenantId)
    {
        WebhookLogId = webhookLogId;
        WebhookDefinitionId = webhookDefinitionId;
        ErrorMessage = errorMessage;
        AttemptNumber = attemptNumber;
        TenantId = tenantId;
    }
}