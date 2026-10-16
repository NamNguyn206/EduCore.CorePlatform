using EduCore.CorePlatform.Domain.Common.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Domain.Events.Domain.SuperAdmin;

public sealed class SuperAdminBootstrappedEvent : DomainEvent
{
    public Guid SuperAdminId { get; }

    public SuperAdminBootstrappedEvent(Guid systemAdminId)
    {
        SuperAdminId = systemAdminId;
    }
}
