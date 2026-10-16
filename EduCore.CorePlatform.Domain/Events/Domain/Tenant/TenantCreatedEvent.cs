using EduCore.CorePlatform.Domain.Common.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Domain.Events.Domain.Tenant;

public class TenantCreatedEvent : DomainEvent
{
    public Guid Id { get; }
    public string Code { get; }
    public string Name { get; }    

    public TenantCreatedEvent(Guid id, string code, string name)
    {
        Id = id;
        Code = code;
        Name = code;
    }
}