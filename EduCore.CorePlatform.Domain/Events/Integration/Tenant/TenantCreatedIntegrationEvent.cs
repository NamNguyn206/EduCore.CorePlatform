using EduCore.CorePlatform.Domain.Common.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Domain.Events.Integration.TenantIntegration;

public class TenantCreatedIntegrationEvent : IntegrationEvent
{
    public Guid TenantId { get; }
    public string TenantName { get; }
    public string TenantCode { get; }
    public string? Email { get; }
    public string? Phone { get; }
    public DateTime CreatedAt { get; }

    public TenantCreatedIntegrationEvent(Guid tenantId, string tenantName, string tenantCode,
                                        string? email, string? phone, DateTime createdAt)
    {
        TenantId = tenantId;
        TenantName = tenantName;
        TenantCode = tenantCode;
        Email = email;
        Phone = phone;
        CreatedAt = createdAt;
    }
}