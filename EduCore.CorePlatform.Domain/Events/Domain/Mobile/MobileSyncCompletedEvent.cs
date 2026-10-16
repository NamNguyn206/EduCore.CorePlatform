using EduCore.CorePlatform.Domain.Common.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Domain.Events.Domain.Mobile;

public class MobileSyncCompletedEvent : DomainEvent
{
    public Guid UserId { get; }
    public string DeviceId { get; }
    public Guid? TenantId { get; }
    public Guid SyncBatchId { get; }
    public bool IsSuccess { get; }
    public int SyncedEntitiesCount { get; }

    public MobileSyncCompletedEvent(Guid userId, string deviceId, Guid syncBatchId,
                                   bool isSuccess, int syncedEntitiesCount, Guid? tenantId = null)
    {
        UserId = userId;
        DeviceId = deviceId;
        SyncBatchId = syncBatchId;
        IsSuccess = isSuccess;
        SyncedEntitiesCount = syncedEntitiesCount;
        TenantId = tenantId;
    }
}