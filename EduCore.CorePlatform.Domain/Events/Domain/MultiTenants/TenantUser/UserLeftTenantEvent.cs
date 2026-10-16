using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Domain.Common.Events;

namespace EduCore.CorePlatform.Domain.Events.Domain.MultiTenants.TenantUser;

public class UserLeftTenantEvent : DomainEvent
{
    public Guid UserId { get; }
    public Guid TenantId { get; }

    public UserLeftTenantEvent(Guid userId, Guid tenantId)
    {
        UserId = userId;
        TenantId = tenantId;
    }
}