using EduCore.CorePlatform.Domain.Common.Constants;
using EduCore.CorePlatform.Domain.Common.Entities;
using EduCore.CorePlatform.Domain.Common.Enums.System.Webhook;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace EduCore.CorePlatform.Domain.Entities.System.Webhooks;

public class WebhookDefinition : MultiTenantEntity<Guid, Guid>
{    
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public string TargetUrl { get; private set; } = string.Empty;
    public WebhookEventType EventType { get; private set; }
    public WebhookStatus Status { get; private set; } = WebhookStatus.Active;
    public string? Secret { get; private set; }
    public bool IncludePayload { get; private set; } = true;
    public bool IsActive { get; private set; } = true;
    public int RetryCount { get; private set; } = 3;
    public int RetryIntervalSeconds { get; private set; } = 60;
    public string? Headers { get; private set; } // JSON key-value pairs
    public string? Filters { get; private set; } // JSON filter conditions
    public DateTime? LastTriggeredAt { get; private set; }
    public int SuccessCount { get; private set; }
    public int FailureCount { get; private set; }

    // Navigation properties
    public virtual ICollection<WebhookSubscription> Subscriptions { get; private set; } = new List<WebhookSubscription>();
    public virtual ICollection<WebhookLog> Logs { get; private set; } = new List<WebhookLog>();

    protected WebhookDefinition() { }

    public WebhookDefinition(Guid tenantId, string name, string targetUrl,
                           WebhookEventType eventType, string? description = null)
        : base(Guid.NewGuid(), tenantId)
    {
        Name = name;
        TargetUrl = targetUrl;
        EventType = eventType;
        Description = description;
        CreatedAt = DateTime.UtcNow;
        Status = WebhookStatus.Active;
        IsActive = true;
        Secret = Guid.NewGuid().ToString("N");
    }

    public void Update(string name, string targetUrl, string? description,
                      bool includePayload, int retryCount, int retryIntervalSeconds)
    {
        Name = name;
        TargetUrl = targetUrl;
        Description = description;
        IncludePayload = includePayload;
        RetryCount = retryCount;
        RetryIntervalSeconds = retryIntervalSeconds;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetHeaders(Dictionary<string, string> headers)
    {
        Headers = JsonSerializer.Serialize(headers);
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetFilters(Dictionary<string, object> filters)
    {
        Filters = JsonSerializer.Serialize(filters);
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        Status = WebhookStatus.Active;
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        Status = WebhookStatus.Inactive;
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsError()
    {
        Status = WebhookStatus.Error;
        UpdatedAt = DateTime.UtcNow;
    }

    public void RecordSuccess()
    {
        LastTriggeredAt = DateTime.UtcNow;
        SuccessCount++;
        Status = WebhookStatus.Active;
        UpdatedAt = DateTime.UtcNow;
    }

    public void RecordFailure()
    {
        LastTriggeredAt = DateTime.UtcNow;
        FailureCount++;

        if (FailureCount > 10)
        {
            Status = WebhookStatus.Error;
        }

        UpdatedAt = DateTime.UtcNow;
    }

    public void ResetCounters()
    {
        SuccessCount = 0;
        FailureCount = 0;
        Status = WebhookStatus.Active;
        UpdatedAt = DateTime.UtcNow;
    }

    public void RegenerateSecret()
    {
        Secret = Guid.NewGuid().ToString("N");
        UpdatedAt = DateTime.UtcNow;
    }
}