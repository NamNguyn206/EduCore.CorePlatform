using EduCore.CorePlatform.Domain.Common.Constants;
using EduCore.CorePlatform.Domain.Common.Entities;
using EduCore.CorePlatform.Domain.Common.Enums.System.MobileApp;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EduCore.CorePlatform.Domain.Entities.System.MobileApp;

public class MobileAppSyncLog : BaseEntity<Guid>
{
    public Guid? TenantId { get; private set; }    
    public Guid UserId { get; private set; }
    public string DeviceId { get; private set; } = string.Empty;    
    public MobileAppSyncOperation Operation { get; private set; }    
    public string EntityType { get; private set; } = string.Empty;
    public Guid? EntityId { get; private set; }
    public string? EntityData { get; private set; }
    public string? Changes { get; private set; }    
    public DateTime SyncTimestamp { get; private set; }
    public bool IsSuccess { get; private set; }
    public string? ErrorMessage { get; private set; }
    public Guid? SyncBatchId { get; private set; }
    public string? AppVersion { get; private set; }
    public string? OsVersion { get; private set; }
    public string? Metadata { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    protected MobileAppSyncLog() { }

    public MobileAppSyncLog(Guid? tenantId, Guid userId, string deviceId, MobileAppSyncOperation operation,
                        string entityType, Guid? entityId, DateTime syncTimestamp, Guid? syncBatchId = null)
    {
        Id = Guid.NewGuid();
        TenantId = tenantId;
        UserId = userId;
        DeviceId = deviceId;
        Operation = operation;
        EntityType = entityType;
        EntityId = entityId;
        SyncTimestamp = syncTimestamp;
        SyncBatchId = syncBatchId;
        CreatedAt = DateTime.UtcNow;
    }

    public void RecordSuccess(string? entityData = null, string? changes = null, string? metadata = null)
    {
        IsSuccess = true;
        EntityData = entityData;
        Changes = changes;
        Metadata = metadata;
        UpdatedAt = DateTime.UtcNow;
    }

    public void RecordFailure(string errorMessage, string? entityData = null, string? changes = null)
    {
        IsSuccess = false;
        ErrorMessage = errorMessage;
        EntityData = entityData;
        Changes = changes;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetDeviceInfo(string appVersion, string osVersion)
    {
        AppVersion = appVersion;
        OsVersion = osVersion;
        UpdatedAt = DateTime.UtcNow;
    }
}