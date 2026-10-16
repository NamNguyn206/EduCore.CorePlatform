using EduCore.CorePlatform.Domain.Common.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Domain.Events.Integration.TenantIntegration;

public class TenantUpdatedIntegrationEvent : IntegrationEvent
{
    public Guid TenantId { get; }
    public string TenantName { get; }
    public string? Email { get; }
    public string? Phone { get; }
    public DateTime UpdatedAt { get; }

    public TenantUpdatedIntegrationEvent(Guid tenantId, string tenantName,
                                        string? email, string? phone, DateTime updatedAt)
    {
        TenantId = tenantId;
        TenantName = tenantName;
        Email = email;
        Phone = phone;
        UpdatedAt = updatedAt;
    }
}