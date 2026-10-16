using EduCore.CorePlatform.Domain.Common.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Domain.Events.Integration.User;

public class UserDeletedIntegrationEvent : IntegrationEvent
{
    public Guid UserId { get; }
    public string Username { get; }
    public DateTime DeletedAt { get; }
    public Guid? DeletedBy { get; }

    public UserDeletedIntegrationEvent(Guid userId, string username,
                                      DateTime deletedAt, Guid? deletedBy)
    {
        UserId = userId;
        Username = username;
        DeletedAt = deletedAt;
        DeletedBy = deletedBy;
    }
}