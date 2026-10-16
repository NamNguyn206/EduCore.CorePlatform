using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Domain.Common.Events;

namespace EduCore.CorePlatform.Domain.Events.Domain.User;

public class UserLanguageChangedEvent : DomainEvent
{
    public Guid UserId { get; }
    public Guid LanguageId { get; }
    public Guid? UpdatedByUserId { get; }

    public UserLanguageChangedEvent(Guid userId, Guid languageId, Guid? updatedByUserId)
    {
        UserId = userId;
        LanguageId = languageId;
        UpdatedByUserId = updatedByUserId;
    }
}