using EduCore.CorePlatform.Domain.Common.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Domain.Events.Domain.Mobile;

public class MobileSyncStartedEvent : DomainEvent
{
    public Guid UserId { get; }
    public string DeviceId { get; }
    public Guid? TenantId { get; }
    public Guid? SyncBatchId { get; }

    public MobileSyncStartedEvent(Guid userId, string deviceId, Guid? tenantId = null,
                                 Guid? syncBatchId = null)
    {
        UserId = userId;
        DeviceId = deviceId;
        TenantId = tenantId;
        SyncBatchId = syncBatchId;
    }
}