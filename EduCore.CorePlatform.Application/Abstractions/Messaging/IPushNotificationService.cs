using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace EduCore.CorePlatform.Application.Abstractions.Messaging;

public interface IPushNotificationService
{
    Task UnregisterDeviceAsync(string deviceToken, CancellationToken cancellationToken = default);
 
    Task SubscribeToTopicAsync(string deviceToken, string topic, CancellationToken cancellationToken = default);
    Task UnsubscribeFromTopicAsync(string deviceToken, string topic, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<string>> GetDeviceTopicsAsync(string deviceToken, CancellationToken cancellationToken = default);

 
    Task MarkAsReadAsync(Guid notificationId, Guid userId, CancellationToken cancellationToken = default);
    Task MarkAllAsReadAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<PushDeliveryStatus> GetDeliveryStatusAsync(Guid notificationId, CancellationToken cancellationToken = default);
 
    Task TestConnectionAsync(CancellationToken cancellationToken = default);
 
}