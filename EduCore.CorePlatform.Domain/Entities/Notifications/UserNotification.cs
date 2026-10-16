using EduCore.CorePlatform.Domain.Common.Entities;
using EduCore.CorePlatform.Domain.Common.Enums.Notifications;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EduCore.CorePlatform.Domain.Entities.Notifications;

public class UserNotification : MultiTenantEntity<Guid, Guid>
{    
    public Guid UserId { get; private set; }    
    public Guid NotificationId { get; private set; }    
    public UserNotificationStatus Status { get; private set; } = UserNotificationStatus.Unread;
    public DateTime? ReadAt { get; private set; }

    // Navigation properties
    public virtual Identity.User User { get; private set; } = null!;
    public virtual Notification Notification { get; private set; } = null!;

    protected UserNotification() { }

    public UserNotification(Guid tenantId, Guid userId, Guid notificationId)
        : base(Guid.NewGuid(), tenantId)
    {
        UserId = userId;
        NotificationId = notificationId;
        Status = UserNotificationStatus.Unread;
        CreatedAt = DateTime.UtcNow;
    }

    public void MarkAsRead()
    {
        Status = UserNotificationStatus.Read;
        ReadAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsUnread()
    {
        Status = UserNotificationStatus.Unread;
        ReadAt = null;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsArchived()
    {
        Status = UserNotificationStatus.Archived;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Delete()
    {
        DeletedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Restore()
    {
        DeletedAt = null;
        UpdatedAt = DateTime.UtcNow;
    }
}

