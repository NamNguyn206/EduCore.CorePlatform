using EduCore.CorePlatform.Domain.Common.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Domain.Events.Domain.Tenant;

public class TenantUpdatedEvent : DomainEvent
{
    public Guid Id { get; }

    public TenantUpdatedEvent(Guid id)
    {
        Id = id;
    }
}