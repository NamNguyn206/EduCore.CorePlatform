using EduCore.CorePlatform.Domain.Common.Constants;
using EduCore.CorePlatform.Domain.Common.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace EduCore.CorePlatform.Domain.Entities.System.Webhooks;

public class WebhookLog : MultiTenantEntity<Guid, Guid>
{
    public Guid WebhookDefinitionId { get; private set; }    
    public Guid WebhookEventId { get; private set; }
    
    public DateTime TriggeredAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    public int AttemptNumber { get; private set; } = 1;
    public string? TargetUrl { get; private set; }
    public int StatusCode { get; private set; }
    public string? RequestBody { get; private set; }
    public string? ResponseBody { get; private set; }
    public string? RequestHeaders { get; private set; }
    public string? ResponseHeaders { get; private set; }
    public string? ErrorMessage { get; private set; }
    public TimeSpan? Duration { get; private set; }
    public bool IsSuccess => StatusCode >= 200 && StatusCode < 300;
    public bool IsFailed => !IsSuccess;

    // Navigation properties
    public virtual WebhookDefinition WebhookDefinition { get; private set; } = null!;
    public virtual WebhookEvent WebhookEvent { get; private set; } = null!;

    protected WebhookLog() { }

    public WebhookLog(Guid tenantId, Guid webhookDefinitionId, Guid webhookEventId,
                     string targetUrl, string? requestBody = null)
        : base(Guid.NewGuid(), tenantId)
    {
        WebhookDefinitionId = webhookDefinitionId;
        WebhookEventId = webhookEventId;
        TargetUrl = targetUrl;
        RequestBody = requestBody;
        TriggeredAt = DateTime.UtcNow;
    }

    public void RecordSuccess(int statusCode, string? responseBody, string? responseHeaders, TimeSpan duration)
    {
        StatusCode = statusCode;
        ResponseBody = responseBody;
        ResponseHeaders = responseHeaders;
        Duration = duration;
        CompletedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void RecordFailure(int statusCode, string? responseBody, string? responseHeaders,
                            string? errorMessage, TimeSpan duration)
    {
        StatusCode = statusCode;
        ResponseBody = responseBody;
        ResponseHeaders = responseHeaders;
        ErrorMessage = errorMessage;
        Duration = duration;
        CompletedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetRequestHeaders(Dictionary<string, string> headers)
    {
        RequestHeaders = JsonSerializer.Serialize(headers);
    }

    public void IncrementAttempt()
    {
        AttemptNumber++;
        UpdatedAt = DateTime.UtcNow;
    }
}