using EduCore.CorePlatform.Domain.Common.Constants;
using EduCore.CorePlatform.Domain.Common.Entities;
using EduCore.CorePlatform.Domain.Common.Enums.Notifications;
using EduCore.CorePlatform.Domain.Common.Events;
using EduCore.CorePlatform.Domain.Entities.System.Files;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace EduCore.CorePlatform.Domain.Entities.Notifications;

public class Notification : MultiTenantEntity<Guid, Guid>
{
    private readonly List<DomainEvent> _domainEvents = new();
    public string Title { get; private set; } = string.Empty;
    public string Message { get; private set; } = string.Empty;
    public string? Data { get; private set; }
    public NotificationType Type { get; private set; }
    public NotificationPriority Priority { get; private set; } = NotificationPriority.Normal;    
    public NotificationStatus Status { get; private set; } = NotificationStatus.Pending;
    public Guid? SenderId { get; private set; }
    public string? SenderName { get; private set; }
    public DateTime ScheduledFor { get; private set; }
    public DateTime? SentAt { get; private set; }
    public DateTime? ReadAt { get; private set; }
    public string? Recipients { get; private set; } // JSON array of recipient IDs or emails
    public bool IsBroadcast { get; private set; }
    public string? Channel { get; private set; } // Email, SMS, Push, InApp
    public string? Attachments { get; private set; } // JSON array of attachment info

    // Navigation properties
    public virtual ICollection<UserNotification> UserNotifications { get; private set; } = new List<UserNotification>();
    public virtual ICollection<FileStorage> FileAttachments { get; private set; } = new List<FileStorage>();

    protected Notification() { }

    public Notification(Guid tenantId, string title, string message, NotificationType type,
                       DateTime scheduledFor, Guid? senderId = null, string? senderName = null,
                       bool isBroadcast = false, string? channel = "InApp")
        : base(Guid.NewGuid(), tenantId)
    {
        Title = title;
        Message = message;
        Type = type;
        ScheduledFor = scheduledFor;
        SenderId = senderId;
        SenderName = senderName;
        IsBroadcast = isBroadcast;
        Channel = channel;
        CreatedAt = DateTime.UtcNow;
        Status = NotificationStatus.Pending;
    }

    public void MarkAsSent()
    {
        Status = NotificationStatus.Sent;
        SentAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsRead()
    {
        Status = NotificationStatus.Read;
        ReadAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsFailed()
    {
        Status = NotificationStatus.Failed;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetRecipients(List<Guid> userIds)
    {
        Recipients = JsonSerializer.Serialize(userIds);
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetRecipientsByEmail(List<string> emails)
    {
        Recipients = JsonSerializer.Serialize(emails);
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetData(object data)
    {
        Data = JsonSerializer.Serialize(data);
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetAttachments(List<FileStorage> attachments)
    {
        Attachments = JsonSerializer.Serialize(attachments);
        UpdatedAt = DateTime.UtcNow;
    }

    public void Reschedule(DateTime newScheduledTime)
    {
        ScheduledFor = newScheduledTime;
        Status = NotificationStatus.Pending;
        UpdatedAt = DateTime.UtcNow;
    }

    public List<Guid> GetUserRecipients()
    {
        if (string.IsNullOrEmpty(Recipients))
            return new List<Guid>();

        return JsonSerializer.Deserialize<List<Guid>>(Recipients) ?? new List<Guid>();
    }
}