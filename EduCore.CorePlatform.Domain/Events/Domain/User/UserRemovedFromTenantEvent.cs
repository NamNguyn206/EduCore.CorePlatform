using EduCore.CorePlatform.Domain.Common.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Domain.Events.Domain.User;

public class UserRemovedFromTenantEvent : DomainEvent
{
    public Guid UserId { get; }
    public Guid TenantId { get; }

    public UserRemovedFromTenantEvent(Guid userId, Guid tenantId)
    {
        UserId = userId;
        TenantId = tenantId;
    }
}