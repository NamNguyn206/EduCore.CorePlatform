using EduCore.CorePlatform.Domain.Common.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Domain.Events.Integration.TenantIntegration;

public class TenantDeletedIntegrationEvent : IntegrationEvent
{
    public Guid TenantId { get; }
    public string TenantName { get; }
    public DateTime DeletedAt { get; }
    public Guid? DeletedBy { get; }

    public TenantDeletedIntegrationEvent(Guid tenantId, string tenantName,
                                        DateTime deletedAt, Guid? deletedBy)
    {
        TenantId = tenantId;
        TenantName = tenantName;
        DeletedAt = deletedAt;
        DeletedBy = deletedBy;
    }
}