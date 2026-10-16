using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Abstractions.Messaging;
public class PushDeliveryStatus
{
    public Guid NotificationId { get; set; }
    public string Status { get; set; } = string.Empty;
    public int SentCount { get; set; }
    public int DeliveredCount { get; set; }
    public int FailedCount { get; set; }
    public DateTime? SentAt { get; set; }
    public string? ErrorMessage { get; set; }
}