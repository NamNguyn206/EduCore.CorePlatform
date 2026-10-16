using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Domain.Common.Events;

namespace EduCore.CorePlatform.Domain.Events.Domain.Localization;

public class LanguageUpdatedEvent : DomainEvent
{
    public Guid Id { get; }
    public Guid? UpdatedByUserId { get; }

    public LanguageUpdatedEvent(Guid id, Guid? updatedByUserId = null)
    {
        Id = id;
        UpdatedByUserId = updatedByUserId;
    }
}