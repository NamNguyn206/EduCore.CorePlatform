using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Domain.Common.Events;

namespace EduCore.CorePlatform.Domain.Events.Domain.Localization;

public class LanguageCreatedEvent : DomainEvent
{
    public Guid Id { get; }
    public string Code { get; }
    public string Name { get; }
    public Guid? CreatedByUserId { get; }

    public LanguageCreatedEvent(Guid id, string code, string name, Guid? createdByUserId = null)
    {
        Id = id;
        Code = code;
        Name = code;
        CreatedByUserId = createdByUserId;
    }
}