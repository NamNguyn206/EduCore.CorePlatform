using EduCore.CorePlatform.Domain.Common.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Domain.Events.Domain.User;

public class UserRoleChangedEvent : DomainEvent
{
    public Guid UserId { get; }
    public Guid? OldRoleId { get; }
    public Guid NewRoleId { get; }
    public Guid? TenantId { get; }

    public UserRoleChangedEvent(Guid userId, Guid? oldRoleId, Guid newRoleId, Guid? tenantId = null)
    {
        UserId = userId;
        OldRoleId = oldRoleId;
        NewRoleId = newRoleId;
        TenantId = tenantId;
    }
}