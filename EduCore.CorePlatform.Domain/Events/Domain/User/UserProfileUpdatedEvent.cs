using EduCore.CorePlatform.Domain.Common.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Domain.Events.Domain.User;

public class UserProfileUpdatedEvent : DomainEvent
{
    public Guid UserId { get; }

    public UserProfileUpdatedEvent(Guid userId)
    {
        UserId = userId;
    }
}